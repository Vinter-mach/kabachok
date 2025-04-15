from aiogram.types import ReplyKeyboardMarkup, KeyboardButton, \
    InlineKeyboardMarkup, InlineKeyboardButton
from tgBot.database.db import get_lesson


def get_KeyboardButton_for_lesson():
    lessons = get_lesson()
    keyboardButton = []
    for lesson_name in lessons:
        keyboardButton.append([KeyboardButton(text=lesson_name)])
    return keyboardButton


confirm_kb = ReplyKeyboardMarkup(
    keyboard=[
        [KeyboardButton(text="Да")],
        [KeyboardButton(text="Нет")]
    ],
    resize_keyboard=True
)

lesson_kb = ReplyKeyboardMarkup(
    keyboard=get_KeyboardButton_for_lesson(),
    resize_keyboard=True
)

lesson_action_kb = ReplyKeyboardMarkup(
    keyboard=[
        [KeyboardButton(text="Отправить"),
         KeyboardButton(text="Посмотреть статус")]
    ],
    resize_keyboard=True
)

send_or_select_topic = ReplyKeyboardMarkup(
    keyboard=[
        [KeyboardButton(text="Выбрать другую тему")],
        [KeyboardButton(text="Отправить задание")]
    ],
    resize_keyboard=True
)
