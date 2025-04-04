from aiogram import Router, types
from aiogram.fsm.context import FSMContext
from tgBot.states.register import Lesson
from tgBot.keyboards.reply import lesson_kb, lesson_action_kb
from tgBot.database.db import lessons

router = Router()



@router.message(Lesson.choosing_topic)
async def choose_topic(message: types.Message, state: FSMContext):
    text = message.text.lower()
    if text in lessons:
        await state.set_state(Lesson.after_topic)
        await message.answer(lessons[text], reply_markup=lesson_action_kb)
    else:
        await message.answer("Выбери тему:", reply_markup=lesson_kb)

@router.message(Lesson.after_topic)
async def after_topic(message: types.Message, state: FSMContext):
    if message.text in ["Отправить", "Посмотреть статус"]:
        await message.answer(f"Вы выбрали: {message.text}")
    else:
        await message.answer("Выбери тему:", reply_markup=lesson_kb)
        await state.set_state(Lesson.choosing_topic)
