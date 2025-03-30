from aiogram import Router, types
from aiogram.filters import Command
from aiogram.fsm.context import FSMContext

from tgBot.states.register import Register

router = Router()


@router.message(Command("help"))
async def cmd_help(message: types.Message):
    await message.answer(
        "Команды:\n"
        "/help\n"
        "/start"
    )


@router.message(Command("start"))
async def cmd_start(message: types.Message, state: FSMContext):
    await message.answer(
        "Привет! Отправь свою группу и ФИ в формате ФТ-20*-* Фамилия Имя, например, ФТ-203-1 Эшитов Радик"
    )
    await state.set_state(Register.waiting_for_fullname)
