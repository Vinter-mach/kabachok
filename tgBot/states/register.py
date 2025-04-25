from aiogram.fsm.state import State, StatesGroup


class Register(StatesGroup):
    waiting_for_fullname = State()
    confirm_data = State()


class Lesson(StatesGroup):
    choosing_topic = State()
    after_topic = State()
    waiting_for_file = State()


class CourseSelect(StatesGroup):
    waiting_for_course = State()


class LessonSelect(StatesGroup):
    waiting_for_topic = State()
    after_topic = State()
    waiting_for_files = State()


class GravesSelect(StatesGroup):
    waiting_for_topic = State()
    after_topic = State()
    waiting_for_files = State()
