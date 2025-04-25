from aiogram import Router, types
from aiogram.filters import Command
from aiogram.fsm.context import FSMContext
from aiogram.types import KeyboardButton, ReplyKeyboardMarkup

from tgBot.database.request import get_available_courses_for_student
from tgBot.states.register import Lesson, CourseSelect, LessonSelect
from tgBot.handlers.course import show_course_topics

router = Router()


@router.message(Command("help"))
async def cmd_help(message: types.Message):
    await message.answer(
        "/help - помощь\n"
        "/choose_course - выбрать курс\n"
        "/get_lesson - посмотреть темы домашних заданий\n"
        "/get_graves - посмотреть гробы"
    )


@router.message(Command("start"))
async def cmd_help(message: types.Message):
    await message.answer(
        "Привет, я бот для отправок домашних работ и гробов по теории вероятности и математической статистике\n"
        "Узнать о моем функционале: /help"
    )


@router.message(Command("get_lesson"))
async def get_lesson(message: types.Message, state: FSMContext):
    data = await state.get_data()
    if "is_graves" in data:
        data["is_graves"] = False
        print(data["is_graves"])
    if "course_id" not in data:
        courses = await get_available_courses_for_student(message.from_user.id)

        if not courses:
            await message.answer(
                "У тебя нет доступных курсов. Обратись к преподавателю.")
            return

        course_map = {course.name: course.id for course in courses}
        await state.update_data(course_map=course_map)
        buttons = [[KeyboardButton(text=name)] for name in course_map.keys()]
        kb = ReplyKeyboardMarkup(keyboard=buttons, resize_keyboard=True)
        await message.answer("Ты ещё не выбрал курс. Выбери один из доступных:",
                             reply_markup=kb)
        await state.set_state(CourseSelect.waiting_for_course)
    else:
        course_id = data["course_id"]
        await show_course_topics(message, course_id, state)
        await state.set_state(LessonSelect.waiting_for_topic)


@router.message(Command("choose_course"))
async def get_my_course(message: types.Message, state: FSMContext):
    courses = await get_available_courses_for_student(message.from_user.id)

    if not courses:
        await message.answer(
            "У тебя пока нет доступных курсов. Обратись к преподавателю.")
        return

    course_map = {course.name: course.id for course in courses}
    await state.update_data(course_map=course_map)

    buttons = [[KeyboardButton(text=course.name)] for course in courses]
    kb = ReplyKeyboardMarkup(keyboard=buttons, resize_keyboard=True)

    await message.answer("Вот твои доступные курсы", reply_markup=kb)
    await state.set_state(CourseSelect.waiting_for_course)


@router.message(Command("get_graves"))
async def get_graves(message: types.Message, state: FSMContext):
    pass
    #await state.update_data(is_graves=True)
    #await get_lesson(message, state)
