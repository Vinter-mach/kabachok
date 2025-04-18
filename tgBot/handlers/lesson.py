import asyncio
from collections import defaultdict
from aiogram.fsm.context import FSMContext
from aiogram import Router, types, F
from aiogram.types import ReplyKeyboardRemove
from tgBot.database.request import get_last_submission_full, \
    get_task_id_by_topic_name, save_submission_to_db, has_student_submitted, \
    get_task_info_by_id
from tgBot.handlers.course import show_course_topics
from tgBot.keyboards.reply import send_or_select_topic
from tgBot.states.register import LessonSelect
from tgBot.utils.auth import get_mask_for_save

router = Router()
album_cache: dict[str, list[types.Message]] = defaultdict(list)


@router.message(LessonSelect.waiting_for_topic)
async def handle_topic_selection(message: types.Message, state: FSMContext):
    data = await state.get_data()
    student_id = data.get("student_id")
    tasks = data["tasks"]
    course_id = data.get("course_id")
    topic_name = message.text.strip()

    if topic_name not in tasks:
        await message.answer("Такой темы нет. Выбери из списка.")
        return
    task_id = await get_task_id_by_topic_name(topic_name, course_id)
    await state.update_data(task_id=task_id)
    await state.update_data(topic_name=topic_name)
    submitted_task = await has_student_submitted(student_id, task_id)
    if not submitted_task:
        task = await get_task_info_by_id(task_id)
        if task:
            await message.answer(
                f"Ты еще не отправлял домашку по этой теме\n"
                f"📚 Тема: {task.topic}\n"
                f"🔗 Ссылка: {task.task_link}\n"
                f"📅 Дедлайн: {task.deadline.strftime('%d.%m.%Y') if task.deadline else '—'}\n"
                f"👤 Преподаватель: {task.teacher.name}"
            )
        else:
            await message.answer("Задание не найдено.")
    else:
        await print_task_information(message, state)

    await message.answer("Что ты хочешь сделать дальше?",
                         reply_markup=send_or_select_topic)
    await state.set_state(LessonSelect.after_topic)


async def print_task_information(message: types.Message, state: FSMContext):
    data = await state.get_data()
    task_id = data["task_id"]
    student_id = data.get("student_id")
    submission = await get_last_submission_full(student_id, task_id)

    topic = submission.task.topic
    deadline = submission.task.deadline
    teacher_name = submission.task.teacher.name
    comment = submission.comment
    status_name = submission.status.name
    grade = submission.grade
    sent_at = submission.submitted_date.strftime("%d.%m.%Y %H:%M")

    if status_name == "Отправлено на проверку":
        await message.answer(
            f"📚 Тема: {topic}\n"
            f"📅 Дедлайн: {deadline}\n"
            f"👤 Преподаватель: {teacher_name}\n"
            f"📌 Статус: {status_name}\n"
            f"📨 Отправлено: {sent_at}"
        )
    else:
        await message.answer(
            f"📚 Тема: {topic}\n"
            f"📅 Дедлайн: {deadline}\n"
            f"👤 Преподаватель: {teacher_name}\n"
            f"📌 Статус: {status_name}\n"
            f"📨 Отправлено: {sent_at}"
            f"📝 Оценка: {grade}\n"
            f"💬 Комментарий: {comment}\n"
        )


@router.message(LessonSelect.after_topic, F.text == "Выбрать другую тему")
async def handle_reselect_topic(message: types.Message, state: FSMContext):
    data = await state.get_data()
    course_id = data.get("course_id")
    await show_course_topics(message, course_id, state)
    await state.set_state(LessonSelect.waiting_for_topic)


@router.message(LessonSelect.after_topic, F.text == "Отправить задание")
async def handle_send_homework(message: types.Message, state: FSMContext):
    await message.answer("Отправь задание одним сообщением",
                         reply_markup=ReplyKeyboardRemove())
    await state.set_state(LessonSelect.waiting_for_files)


@router.message(LessonSelect.waiting_for_files, F.media_group_id)
async def handle_get_album(message: types.Message, state: FSMContext):
    media_group_id = str(message.media_group_id)
    data = await state.get_data()
    album_cache = data.get("media_group", {})

    album_cache.setdefault(media_group_id, []).append(message)
    await state.update_data(media_group=album_cache)

    # Ждём, пока Telegram пришлёт все части альбома
    await asyncio.sleep(1)

    # Повторно получаем данные
    data = await state.get_data()
    messages = data.get("media_group", {}).get(media_group_id, [])

    # Только последнее сообщение обрабатывает
    if message.message_id != messages[-1].message_id:
        return

    is_uncorrected_files = False
    files = []
    for msg in messages:
        if msg.document:
            file_name = msg.document.file_name.lower()
            if file_name.endswith(".pdf") or file_name.endswith(".py"):
                files.append({
                    "file_id": msg.document.file_id,
                    "original_file_name": msg.document.file_name,
                    "mask_for_save": await get_mask_for_save(state)
                })
            else:
                is_uncorrected_files = True
                break
    if is_uncorrected_files:
        await message.answer(
            "Ты отправил недопустимые файлы. Принимаются только .pdf и .py. Попробуй еще раз.")
        return

    await after_accepting_files(files, message, state)


@router.message(LessonSelect.waiting_for_files, F.document)
async def handle_get_single_file(message: types.Message, state: FSMContext):
    file_name = message.document.file_name.lower()
    if not (file_name.endswith(".pdf") or file_name.endswith(".py")):
        await message.answer(
            "Ты отправил недопустимые файл. Принимаются только .pdf и .py. Попробуй еще раз.")
        return
    mask_prefix = await get_mask_for_save(state)
    file = {
        "file_id": message.document.file_id,
        "original_file_name": message.document.file_name,
        "mask_for_save": mask_prefix
    }
    await after_accepting_files([file], message, state, mask_prefix)


async def after_accepting_files(files, message, state, mask_prefix):
    data = await state.get_data()
    student_id = data.get("student_id")
    task_id = data.get("task_id")
    await save_submission_to_db(student_id, task_id, mask_prefix)
    await state.update_data(submitted_files=files)
    await print_task_information(message, state)
    await message.answer("Что ты хочешь сделать дальше?",
                         reply_markup=send_or_select_topic)
    await state.set_state(LessonSelect.after_topic)


@router.message(LessonSelect.waiting_for_files)
async def reject_non_files(message: types.Message):
    await message.answer("Пожалуйста, отправь файл формата .pdf или .py.")
