from aiogram import Router, types
from aiogram.fsm.context import FSMContext
from aiogram.filters import Command
from tgBot.states.register import Register
from tgBot.keyboards.reply import confirm_kb
from tgBot.user import User, set_current_user, get_current_user, CURRENT_USER
import re

router = Router()


@router.message(Register.waiting_for_fullname)
async def process_fullname(message: types.Message, state: FSMContext):
    pattern = r"ФТ-20\d-\d\s+[А-ЯЁа-яё]+ [А-ЯЁа-яё]+"
    if not re.match(pattern, message.text):
        await message.answer(
            "Попробуй еще раз! Пример: ФТ-203-1 Эшитов Радик"
        )
        return

    group, fio = message.text.split(maxsplit=1)
    await state.update_data(group=group, fio=fio)

    await message.answer(
        f"Отлично! Ты {fio} из {group}, верно?",
        reply_markup=confirm_kb
    )
    await state.set_state(Register.confirm_data)


@router.message(Register.confirm_data)
async def confirm_data(message: types.Message, state: FSMContext):
    data = await state.get_data()
    if message.text.lower() == "да":
        user = User(
            user_id=message.from_user.id,
            group=data["group"],
            full_name=data["fio"]
        )
        set_current_user(user)
        user = get_current_user()
        await message.answer(f"Ну теперь скажи мне, "
                             f"{user.full_name}, сосал?")
        await state.clear()
    elif message.text.lower() == "нет":
        await message.answer("Окей, попробуем еще раз. Введи снова:")
        await state.set_state(Register.waiting_for_fullname)
    else:
        await message.answer("Пожалуйста, выбери кнопку: Да или Нет.")
