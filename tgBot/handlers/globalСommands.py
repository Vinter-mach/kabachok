from aiogram import Router, types
from aiogram.filters import Command
from aiogram.fsm.context import FSMContext
from aiogram.types import KeyboardButton, ReplyKeyboardMarkup

from tgBot.database.request import get_available_courses_for_student
from tgBot.states.register import Lesson, CourseSelect
from tgBot.keyboards.reply import lesson_kb

router = Router()


@router.message(Command("help"))
async def cmd_help(message: types.Message):
    await message.answer(
        "/help - помощь\n"
        "/choose_course - выбрать курс"
        "/get_lesson - посмотреть темы домашних заданий"
    )


@router.message(Command("start"))
async def cmd_help(message: types.Message):
    await message.answer(
        "Привет, я бот для отправок домашних работ и гробов по теории вероятности и математической статистике\n"
        "Узнать о моем функционале: /help"
    )


@router.message(Command("get_lesson"))
async def cmd_get_lesson(message: types.Message, state: FSMContext):
    await message.answer(
        f"Выбери тему:",
        reply_markup=lesson_kb
    )
    await state.set_state(Lesson.choosing_topic)


@router.message(Command("choose_course"))
async def get_my_course(message: types.Message, state: FSMContext):
    courses = await get_available_courses_for_student(message.from_user.id)

    if not courses:
        await message.answer("У тебя пока нет доступных курсов. Обратись к преподавателю.")
        return

    course_map = {course.name: course.id for course in courses}
    await state.update_data(course_map=course_map)

    buttons = [[KeyboardButton(text=course.name)] for course in courses]
    kb = ReplyKeyboardMarkup(keyboard=buttons, resize_keyboard=True)

    await message.answer("Вот твои доступные курсы", reply_markup=kb)
    await state.set_state(CourseSelect.waiting_for_course)