from aiogram import Router, types
from aiogram.fsm.context import FSMContext
from aiogram.fsm.storage.base import StorageKey

router = Router()

@router.message()
async def fallback_handler(message: types.Message, state: FSMContext):
    key = StorageKey(bot_id=message.bot.id, chat_id=message.chat.id, user_id=message.from_user.id)
    current_state = await state.storage.get_state(key)

    if current_state is None:
        await message.answer("Пожалуйста, не пиши ничего лишнего. Не нужно грузить сервер который держится на картошке\n"
                             "используй /help чтобы посмотреть доступные команды")
