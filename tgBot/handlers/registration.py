from aiogram import Router, types
from aiogram.fsm.context import FSMContext
from aiogram.types import ReplyKeyboardRemove
from tgBot.states.register import Register
from tgBot.keyboards.reply import confirm_kb
from tgBot.database.request import check_student_identity
import re

router = Router()


@router.message(Register.waiting_for_fullname)
async def process_fullname(message: types.Message, state: FSMContext):
    pattern = r"(ФТ-\d{3}-\d)\s+([А-ЯЁа-яё]+ [А-ЯЁа-яё]+)"
    match = re.match(pattern, message.text)

    if not match:
        await message.answer("Попробуй ещё раз! Пример: ФТ-203-1 Эшитов Радик")
        return

    group_name, full_name = match.groups()

    is_valid = await check_student_identity(
        telegram_id=message.from_user.id,
        full_name=full_name,
        group_name=group_name
    )

    if not is_valid:
        await message.answer(
            "У тебя нет доступа к этому сервису")
        await state.clear()
        return

    await state.update_data(group=group_name, fio=full_name)
    await message.answer(
        f"Привет, {full_name} из {group_name}! Всё сходится?",
        reply_markup=confirm_kb
    )
    await state.set_state(Register.confirm_data)


@router.message(Register.confirm_data)
async def confirm_data(message: types.Message, state: FSMContext):
    data = await state.get_data()

    if message.text.lower() == "да":
        await state.update_data(
            user_id=message.from_user.id,
            group=data["group"],
            full_name=data["fio"]
        )
        await message.answer(
            f"Отлично, {data['fio']} из {data['group']}! Ты зарегистрирован.",
            reply_markup=ReplyKeyboardRemove()
        )
        await state.set_state(None)

    elif message.text.lower() == "нет":
        await message.answer(
            "Окей, попробуем еще раз. Введи снова:",
            reply_markup=ReplyKeyboardRemove()
        )
        await state.set_state(Register.waiting_for_fullname)

    else:
        await message.answer("Пожалуйста, выбери кнопку: Да или Нет.")
