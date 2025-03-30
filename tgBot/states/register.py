from aiogram.fsm.state import State, StatesGroup


class Register(StatesGroup):
    waiting_for_fullname = State()
    confirm_data = State()
