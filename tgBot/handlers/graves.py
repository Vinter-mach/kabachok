from aiogram import Router, types
from aiogram.filters import Command
from aiogram.fsm.context import FSMContext
from tgBot.states.register import GravesSelect

router = Router()

@router.message(GravesSelect.waiting_for_topic)
async def after(message: types.Message, state: FSMContext):
    data = await state.get_data()
    task_id = data['task_id']
    topic_name = data['topic_name']
    student_id = data['student_id']
