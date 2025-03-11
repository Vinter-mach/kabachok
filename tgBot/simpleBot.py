import asyncio
from aiogram import Bot, Dispatcher
from aiogram.types import Message
from aiogram.filters import Command

TOKEN = "ТОКЕН в ТГ"
bot = Bot(token=TOKEN)
dp = Dispatcher()


@dp.message(Command("start"))
async def start_command(message: Message):
    await message.answer("Сосал?")


@dp.message(Command("help"))
async def help_command(message: Message):
    await message.answer("\n/start - что бы стать крутым")


async def main():
    await dp.start_polling(bot)


if __name__ == "__main__":
    asyncio.run(main())
