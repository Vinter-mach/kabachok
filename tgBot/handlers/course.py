import asyncio
from collections import defaultdict

from aiogram.fsm.context import FSMContext
from tgBot.database.models import Course, Student
from tgBot.database.connect import async_session
from sqlalchemy import select
from aiogram import Router, types, F
from aiogram.types import (ReplyKeyboardRemove, ReplyKeyboardMarkup,
                           KeyboardButton)
from tgBot.database.request import get_topics_by_course_id, \
    get_last_submission_full, get_task_id_by_topic_name
from tgBot.keyboards.reply import send_or_select_topic
from tgBot.states.register import CourseSelect

router = Router()
album_cache: dict[str, list[types.Message]] = defaultdict(list)


@router.message(CourseSelect.waiting_for_course)
async def handle_course_choice(message: types.Message, state: FSMContext):
    data = await state.get_data()
    course_map: dict = data.get("course_map", {})

    selected_name = message.text.strip()

    if selected_name not in course_map:
        await message.answer("Выбери курс из списка.")
        return

    course_id = course_map[selected_name]
    await state.update_data(course_id=course_id)

    await message.answer(f"Курс «{selected_name}» выбран",
                         reply_markup=ReplyKeyboardRemove())

    await show_course_topics(message, course_id, state)
    await state.set_state(CourseSelect.waiting_for_topic)


async def show_course_topics(message: types.Message, course_id: int,
                             state: FSMContext):
    task_list = await get_topics_by_course_id(course_id)
    task_dict = dict()
    for task in task_list:
        task_dict[task.topic] = task
    await state.update_data(tasks=task_dict)
    if not task_list:
        await message.answer("Тем по этому курсу пока нет.")
        return

    buttons = [[KeyboardButton(text=task.topic)] for task in task_list]
    kb = ReplyKeyboardMarkup(keyboard=buttons, resize_keyboard=True)

    await message.answer("Вот доступные темы:", reply_markup=kb)


@router.message(CourseSelect.waiting_for_topic)
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
    last_submission_task = await get_last_submission_full(student_id, task_id)
    if not last_submission_task:
        await message.answer("Ты ещё не отправлял задание по этой теме.")
    else:
        await state.update_data(current_submission_task=last_submission_task)
        await message.answer(
            f"📄 Тема: {topic_name}\n"
            f"🔗 Ссылка: {last_submission_task.homework_link}\n"
            f"📅 Дата: {last_submission_task.submitted_date.strftime('%d.%m.%Y')}\n"
            f"📝 Оценка: {last_submission_task.grade}\n"
            f"💬 Комментарий: {last_submission_task.comment}"
        )

    await message.answer("Что ты хочешь сделать дальше?",
                         reply_markup=send_or_select_topic)
    await state.set_state(CourseSelect.after_topic)


@router.message(CourseSelect.after_topic, F.text == "Выбрать другую тему")
async def handle_reselect_topic(message: types.Message, state: FSMContext):
    data = await state.get_data()
    course_id = data.get("course_id")
    await show_course_topics(message, course_id, state)
    await state.set_state(CourseSelect.waiting_for_topic)


@router.message(CourseSelect.after_topic, F.text == "Отправить задание")
async def handle_send_homework(message: types.Message, state: FSMContext):
    await message.answer("Отправь задание одним сообщением (до 5 файлов).",
                         reply_markup=ReplyKeyboardRemove())
    await state.set_state(CourseSelect.waiting_for_files)


@router.message(CourseSelect.waiting_for_files)
async def handle_file_upload(message: types.Message, state: FSMContext):
    # Если сообщение является частью альбома (media_group_id не None)
    if message.media_group_id:
        album_cache[message.media_group_id].append(message)
        # Ждём 1 секунду (Telegram может рассылать альбом по частям)
        await asyncio.sleep(1)
        # Если текущее сообщение – последнее в кэше для этого media_group_id,
        # считаем, что альбом получен полностью
        if message is album_cache[message.media_group_id][-1]:
            album = album_cache.pop(message.media_group_id)
            await process_album(album, state)
    else:
        # Если сообщения нет media_group_id – один файл
        await process_album([message], state)


async def process_album(messages: list[types.Message], state: FSMContext):
    file_details = []
    # Проходим по всем сообщениям альбома
    for msg in messages:
        # Если в сообщении документ – берем его
        if msg.document:
            file_details.append({
                "file_id": msg.document.file_id,
                "file_name": msg.document.file_name,
                "mime_type": msg.document.mime_type,
            })
        elif msg.photo:
            # Если фотография – выбираем последнее фото (наилучшего качества)
            photo = msg.photo[-1]
            file_details.append({
                "file_id": photo.file_id,
                "file_name": "photo.jpg",
                "mime_type": "image/jpeg",
            })
    # Теперь file_details содержит список словарей с данными по всем файлам
    # Например, можно сохранить их в базе или передать дальше по сценарию
    await messages[-1].answer(f"Получено {len(file_details)} файл(а). Задание отправлено.")
    await state.clear()
