import asyncio
from aiogram import Bot, Dispatcher
from aiogram.fsm.storage.memory import MemoryStorage
from config import BOT_TOKEN
from handlers import globalСommands, registration


async def main():
    bot = Bot(token=BOT_TOKEN)
    dp = Dispatcher(storage=MemoryStorage())
    dp.include_router(globalСommands.router)
    dp.include_router(registration.router)
    await dp.start_polling(bot)


if __name__ == "__main__":
    asyncio.run(main())
