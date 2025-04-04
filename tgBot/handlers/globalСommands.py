from aiogram import Router, types
from aiogram.filters import Command
from aiogram.fsm.context import FSMContext

from tgBot.states.register import Register, Lesson
from tgBot.keyboards.reply import lesson_kb

router = Router()


@router.message(Command("help"))
async def cmd_help(message: types.Message):
    await message.answer(
        "Команды:\n"
        "/help\n"
        "/start\n"
        "/get_lesson"
    )


@router.message(Command("start"))
async def cmd_start(message: types.Message, state: FSMContext):
    await message.answer(
        "Привет! Отправь свою группу и ФИ в формате ФТ-20*-* Фамилия Имя, например, ФТ-203-1 Эшитов Радик"
    )
    await state.set_state(Register.waiting_for_fullname)


@router.message(Command("get_lesson"))
async def cmd_get_lesson(message: types.Message, state: FSMContext):
    data = await state.get_data()
    fio = data.get("full_name")
    group = data.get("group")

    if not fio or not group:
        await message.answer("Ты ещё не зарегистрирован. Отправь /start и пройди регистрацию.")
        return

    await message.answer(
        f"Выбери тему:",
        reply_markup=lesson_kb
    )
    await state.set_state(Lesson.choosing_topic)
