import asyncio
from sqlalchemy import select
from tgBot.database.connect import async_session
from tgBot.database.models import Group, Student, Course, Task


# async def print_all_groups():
#     async with async_session() as session:
#         result = await session.execute(select(Group))
#         groups = result.scalars().all()
#
#         for group in groups:
#             print(f"{group.id}: {group.name}")
#
# async def check_student_identity(telegram_id: int, full_name: str,
#                                  group_name: str) -> bool:
#     async with async_session() as session:
#         group_result = await session.execute(
#             select(Group).where(Group.name == group_name)
#         )
#         group = group_result.scalars().first()
#
#         if not group:
#             return False
#         student_result = await session.execute(
#             select(Student).where(and_(
#                 Student.telegram_id == telegram_id,
#                 Student.name == full_name,
#                 Student.group_id == group.id
#             ))
#         )
#
#         student = student_result.scalars().first()
#         return student is not None


async def get_student_by_telegram_id(telegram_id: int) -> Student | None:
    async with async_session() as session:
        result = await session.execute(
            select(Student).where(Student.telegram_id == telegram_id)
        )
        return result.scalars().first()


async def get_available_courses_for_student(tg_id: int) -> list[Course]:
    async with async_session() as session:
        # Шаг 1: ищем всех студентов с данным telegram_id
        result = await session.execute(
            select(Student).where(Student.telegram_id == tg_id)
        )
        students = result.scalars().all()

        # Шаг 2: собираем уникальные course_id из этих студентов
        course_ids = {student.course_id for student in students if
                      student.course_id is not None}

        if not course_ids:
            return []

        # Шаг 3: получаем все курсы по этим course_id
        result = await session.execute(
            select(Course).where(Course.id.in_(course_ids))
        )
        return result.scalars().all()


async def get_topics_by_course_id(course_id: int) -> list[Task]:
    async with async_session() as session:
        result = await session.execute(
            select(Task).where(Task.course_id == course_id)
        )
        return result.scalars().all()
