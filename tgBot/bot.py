import asyncio
from aiogram import Bot, Dispatcher
from aiogram.fsm.storage.memory import MemoryStorage
from config import BOT_TOKEN
from tgBot.utils.auth import AuthMiddleware
from handlers import globalСommands, lesson, rolllback, course


async def main():
    bot = Bot(token=BOT_TOKEN)
    dp = Dispatcher(storage=MemoryStorage())
    dp.message.middleware(AuthMiddleware())
    dp.include_router(globalСommands.router)
    dp.include_router(course.router)
    dp.include_router(rolllback.router)
    await dp.start_polling(bot)


if __name__ == "__main__":
    asyncio.run(main())
