from datetime import date, datetime
from sqlalchemy import select, desc
from sqlalchemy.orm import selectinload
from tgBot.database.connect import async_session
from tgBot.database.models import Student, Course, Task, SubmittedTask


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


async def has_student_submitted(student_id: int,
                                task_id: int) -> SubmittedTask | None:
    async with async_session() as session:
        result = await session.execute(
            select(SubmittedTask)
            .where(SubmittedTask.student_id == student_id)
            .where(SubmittedTask.task_id == task_id)
            .limit(1)
        )
        return result.scalars().first()


async def get_task_info_by_id(task_id: int) -> Task | None:
    async with async_session() as session:
        result = await session.execute(
            select(Task)
            .options(selectinload(Task.teacher))  # подгружаем teacher
            .where(Task.id == task_id)
        )
        return result.scalar_one_or_none()


async def get_task_id_by_topic_name(topic_name: str,
                                    course_id: int) -> int | None:
    async with async_session() as session:
        result = await session.execute(
            select(Task.id).where(Task.topic == topic_name,
                                  Task.course_id == course_id)
        )
        task_id = result.scalar()
        return task_id



async def get_student_id_by_telegram_id(tg_id: int) -> int | None:
    async with async_session() as session:
        result = await session.execute(
            select(Student.id).where(Student.telegram_id == tg_id)
        )
        student_id = result.scalar()
        return student_id


async def save_submission_to_db(student_id: int, task_id: int, prefix: str):
    async with async_session() as session:
        result = await session.execute(
            select(SubmittedTask).where(
                SubmittedTask.student_id == student_id,
                SubmittedTask.task_id == task_id
            )
        )
        existing = result.scalar_one_or_none()

        if existing:
            # Обновляем дату и путь (если хочешь)
            existing.submitted_date = datetime.now()
            existing.status_id = 1
        else:
            submission = SubmittedTask(
                student_id=student_id,
                task_id=task_id,
                status_id=1,  # 1 значит на проверке
                homework_prefix=prefix,
                submitted_date=date.today(),
                grade=0,
                comment=""
            )
            session.add(submission)
        await session.commit()


async def get_last_work(student_id: int,
                        task_id: int) -> SubmittedTask | None:
    async with async_session() as session:
        result = await session.execute(
            select(SubmittedTask)
            .options(
                # для каждой найденной отправки сразу подтянем Task и у него — Teacher
                selectinload(SubmittedTask.task)
                .selectinload(Task.teacher),
                # и сразу подтянем Status
                selectinload(SubmittedTask.status),
            )
            .where(SubmittedTask.student_id == student_id,
                   SubmittedTask.task_id == task_id)
            .order_by(desc(SubmittedTask.submitted_date))
            .limit(1)
        )
        return result.scalars().first()