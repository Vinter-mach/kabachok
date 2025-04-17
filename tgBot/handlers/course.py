from aiogram.fsm.context import FSMContext
from aiogram import Router, types
from aiogram.types import (ReplyKeyboardRemove, ReplyKeyboardMarkup,
                           KeyboardButton)
from tgBot.database.request import get_topics_by_course_id
from tgBot.states.register import CourseSelect, LessonSelect

router = Router()


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
    await state.set_state(LessonSelect.waiting_for_topic)


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
