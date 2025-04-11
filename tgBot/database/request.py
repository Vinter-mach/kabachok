import asyncio
from sqlalchemy import select, and_
from tgBot.database.connect import async_session
from tgBot.database.models import Group, Student


async def print_all_groups():
    async with async_session() as session:
        result = await session.execute(select(Group))
        groups = result.scalars().all()

        for group in groups:
            print(f"{group.id}: {group.name}")


async def check_student_identity(telegram_id: int, full_name: str, group_name: str) -> bool:
    async with async_session() as session:
        group_result = await session.execute(
            select(Group).where(Group.name == group_name)
        )
        group = group_result.scalars().first()

        if not group:
            return False
        student_result = await session.execute(
            select(Student).where(and_(
                Student.telegram_id == telegram_id,
                Student.name == full_name,
                Student.group_id == group.id
            ))
        )

        student = student_result.scalars().first()
        return student is not None

if __name__ == "__main__":
    asyncio.run(print_all_groups())
