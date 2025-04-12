from tgBot.database.request import get_student_by_telegram_id
from aiogram import BaseMiddleware
from aiogram.types import Message
from aiogram.fsm.context import FSMContext
from typing import Callable, Dict, Any, Awaitable


class AuthMiddleware(BaseMiddleware):
    async def __call__(
            self,
            handler: Callable[[Message, Dict[str, Any]], Awaitable[Any]],
            event: Message,
            data: Dict[str, Any]
    ) -> Any:
        state: FSMContext = data["state"]
        message: Message = event

        if not await get_or_load_user_from_db(state, message):
            await message.answer(
                "У вас нет доступа к боту. Обратитесь к преподавателю.")
            return

        return await handler(event, data)


async def get_or_load_user_from_db(state: FSMContext, message: Message) -> bool:
    data = await state.get_data()

    # Если данные уже есть — ничего не делаем
    if data.get("fio") and data.get("group") and data.get(
            "user_id") == message.from_user.id:
        return True

    # Пытаемся найти пользователя в базе
    student = await get_student_by_telegram_id(message.from_user.id)
    if not student:
        return False  # доступа нет

    # Сохраняем в FSMContext
    await state.update_data(
        fio=student.name,
        group_id=student.group_id,
        course_id=student.course_id,
        user_id=message.from_user.id
    )
    return True
