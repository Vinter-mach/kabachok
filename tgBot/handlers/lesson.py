import asyncio
from collections import defaultdict
from aiogram.fsm.context import FSMContext
from aiogram import Router, types, F
from aiogram.types import ReplyKeyboardRemove
from tgBot.database.request import get_last_submission_full, \
    get_task_id_by_topic_name
from tgBot.handlers.course import show_course_topics
from tgBot.keyboards.reply import send_or_select_topic
from tgBot.states.register import LessonSelect

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
    last_submission_task = await get_last_submission_full(student_id, task_id)
    if not last_submission_task:
        await message.answer("Ты ещё не отправлял задание по этой теме.")
    else:
        await state.update_data(current_submission_task=last_submission_task)
        await print_task_information(message, state)

    await message.answer("Что ты хочешь сделать дальше?",
                         reply_markup=send_or_select_topic)
    await state.set_state(LessonSelect.after_topic)


async def print_task_information(message: types.Message, state: FSMContext):
    data = await state.get_data()
    student_id = data["student_id"]
    task_id = data["task_id"]
    task = await get_last_submission_full(student_id, task_id)
    await message.answer(
        f"📄 Тема: пиздец\n"
        f"🔗 Ссылка: {task.homework_link}\n"
        f"📅 Дата: {task.submitted_date.strftime('%d.%m.%Y')}\n"
        f"📝 Оценка: {task.grade}\n"
        f"💬 Комментарий: {task.comment}"
    )



@router.message(LessonSelect.after_topic, F.text == "Выбрать другую тему")
async def handle_reselect_topic(message: types.Message, state: FSMContext):
    data = await state.get_data()
    course_id = data.get("course_id")
    await show_course_topics(message, course_id, state)
    await state.set_state(LessonSelect.waiting_for_topic)


@router.message(LessonSelect.after_topic, F.text == "Отправить задание")
async def handle_send_homework(message: types.Message, state: FSMContext):
    await message.answer("Отправь задание одним сообщением (до 5 файлов).",
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
    saved_files = []
    for msg in messages:
        if msg.document:
            file_name = msg.document.file_name.lower()
            if file_name.endswith(".pdf") or file_name.endswith(".py"):
                saved_files.append({
                    "file_id": msg.document.file_id,
                    "file_name": msg.document.file_name
                })
            else:
                is_uncorrected_files = True
                break

    if is_uncorrected_files:
        await message.answer("Ты отправил недопустимые файлы. Принимаются только .pdf и .py. Попробуй еще раз.")
        return

    await state.update_data(submitted_files=saved_files)
    await print_task_information(message, state)
    await message.answer("Что ты хочешь сделать дальше?",
                         reply_markup=send_or_select_topic)
    await state.set_state(LessonSelect.after_topic)


@router.message(LessonSelect.waiting_for_files, F.document)
async def handle_get_single_file(message: types.Message, state: FSMContext):
    file_name = message.document.file_name.lower()
    if not (file_name.endswith(".pdf") or file_name.endswith(".py")):
        await message.answer("Ты отправил недопустимые файлы. Принимаются только .pdf и .py. Попробуй еще раз.")
        return
    file_info = {
        "file_id": message.document.file_id,
        "file_name": message.document.file_name
    }

    await state.update_data(submitted_files=[file_info])
    await print_task_information(message, state)
    await message.answer("Что ты хочешь сделать дальше?",
                         reply_markup=send_or_select_topic)
    await state.set_state(LessonSelect.after_topic)


@router.message(LessonSelect.waiting_for_files)
async def reject_non_files(message: types.Message):
    await message.answer("Пожалуйста, отправь файл формата .pdf или .py.")

