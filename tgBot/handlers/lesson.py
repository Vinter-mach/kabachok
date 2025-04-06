from aiogram import Router, types
from aiogram.fsm.context import FSMContext
from tgBot.states.register import Lesson
from tgBot.keyboards.reply import lesson_kb, lesson_action_kb
from tgBot.database.db import lessons,  save_submission, get_submission
from aiogram import F

router = Router()


@router.message(Lesson.choosing_topic)
async def choose_topic(message: types.Message, state: FSMContext):
    text = message.text.lower()
    if text in lessons:
        await state.set_state(Lesson.after_topic)
        await message.answer(lessons[text], reply_markup=lesson_action_kb)
    else:
        await message.answer("Выбери тему:", reply_markup=lesson_kb)


# @router.message(Lesson.after_topic)
# async def after_topic(message: types.Message, state: FSMContext):
#     if message.text in ["Отправить", "Посмотреть статус"]:
#         await message.answer(f"Вы выбрали: {message.text}")
#     else:
#         await message.answer("Выбери тему:", reply_markup=lesson_kb)
#         await state.set_state(Lesson.choosing_topic)


@router.message(Lesson.after_topic, F.text.lower() == "отправить")
async def handle_send_file_request(message: types.Message, state: FSMContext):
    await message.answer("Хорошо, жду от тебя PDF файл.")
    await state.set_state(Lesson.waiting_for_file)


@router.message(Lesson.after_topic, F.text.lower() == "посмотреть статус")
async def handle_check_status(message: types.Message, state: FSMContext):
    submission = get_submission(message.from_user.id)

    if not submission:
        await message.answer("Ты ещё не загружал работу.")
        return

    await message.answer_document(
        submission["file_id"],
        caption=f"Твоя работа: {submission['filename']}\n"
                f"Статус: {submission['status']}"
    )


@router.message(Lesson.waiting_for_file, F.document)
async def handle_pdf_upload(message: types.Message, state: FSMContext):
    document = message.document

    if not document.file_name.lower().endswith(".pdf"):
        await message.answer("Пожалуйста, отправь именно PDF файл.")
        return

    # Сохраняем в "БД"
    save_submission(
        user_id=message.from_user.id,
        file_id=document.file_id,
        filename=document.file_name
    )

    await message.answer("Файл получен. Мы запомнили твою работу. Статус: на проверке.")
    await state.set_state(Lesson.after_topic)


@router.message(Lesson.waiting_for_file)
async def invalid_file_type(message: types.Message):
    await message.answer("Ожидаю PDF файл. Попробуй ещё раз.")
