from sqlalchemy import Column, Integer, String, Date, Boolean, ForeignKey, \
    Index, DateTime
from sqlalchemy.orm import declarative_base, relationship

Base = declarative_base()


class Group(Base):
    __tablename__ = "groups"
    id = Column(Integer, primary_key=True, autoincrement=True)
    name = Column(String(256), nullable=False)


class Course(Base):
    __tablename__ = "courses"
    id = Column(Integer, primary_key=True, autoincrement=True)
    name = Column(String(256), nullable=False)
    password_hash = Column(String(256), nullable=False)


class Status(Base):
    __tablename__ = "statuses"
    id = Column(Integer, primary_key=True, autoincrement=True)
    name = Column(String(256), nullable=False)

    submitted_tasks = relationship("SubmittedTask", back_populates="status")


class Student(Base):
    __tablename__ = "students"
    id = Column(Integer, primary_key=True, autoincrement=True)
    group_id = Column(Integer, ForeignKey("groups.id", ondelete="CASCADE"),
                      nullable=False)
    name = Column(String(256), nullable=False)
    telegram_id = Column(Integer, nullable=False)
    course_id = Column(Integer, ForeignKey("courses.id", ondelete="CASCADE"),
                       nullable=False)

    submitted_tasks = relationship("SubmittedTask", back_populates="student")
    group = relationship("Group")
    course = relationship("Course")

    __table_args__ = (
        Index("IX_students_course_id", "course_id"),
        Index("IX_students_group_id", "group_id"),
    )


class SubmittedTask(Base):
    __tablename__ = "submitted_tasks"
    id = Column(Integer, primary_key=True, autoincrement=True)
    student_id = Column(Integer, ForeignKey("students.id", ondelete="CASCADE"),
                        nullable=False)
    task_id = Column(Integer, ForeignKey("tasks.id", ondelete="CASCADE"),
                     nullable=False)
    status_id = Column(Integer, ForeignKey("statuses.id", ondelete="CASCADE"),
                       nullable=False)
    homework_prefix = Column(String(256),
                             nullable=False)  # renamed from homework_link
    submitted_date = Column(DateTime, nullable=False)
    grade = Column(Integer, nullable=False)
    comment = Column(String(1000), nullable=False)

    task = relationship("Task", back_populates="submitted_tasks")
    student = relationship("Student", back_populates="submitted_tasks")
    status = relationship("Status", back_populates="submitted_tasks")

    __table_args__ = (
        Index("IX_submitted_tasks_status_id", "status_id"),
        Index("IX_submitted_tasks_student_id", "student_id"),
        Index("IX_submitted_tasks_task_id", "task_id"),
    )


class Task(Base):
    __tablename__ = "tasks"
    id = Column(Integer, primary_key=True, autoincrement=True)
    topic = Column(String(256), nullable=False)
    task_link = Column(String(256), nullable=False)
    deadline = Column(Date, nullable=False)
    teacher_id = Column(Integer, ForeignKey("teachers.id", ondelete="CASCADE"),
                        nullable=False)  # renamed from teacher
    is_grave = Column(Boolean, nullable=False)
    course_id = Column(Integer, ForeignKey("courses.id", ondelete="CASCADE"),
                       nullable=False)

    teacher = relationship("Teacher", back_populates="tasks")
    submitted_tasks = relationship("SubmittedTask", back_populates="task")

    __table_args__ = (
        Index("IX_tasks_course_id", "course_id"),
        Index("IX_tasks_teacher_id", "teacher_id"),
    )


class Teacher(Base):
    __tablename__ = "teachers"
    id = Column(Integer, primary_key=True, autoincrement=True)
    login = Column(String(256), nullable=False)
    name = Column(String(256), nullable=False)
    password_hash = Column(String(256), nullable=False)

    tasks = relationship("Task", back_populates="teacher")


class TeacherCourse(Base):
    __tablename__ = "teacher_courses"
    teacher_id = Column(Integer, ForeignKey("teachers.id", ondelete="CASCADE"),
                        primary_key=True)
    course_id = Column(Integer, ForeignKey("courses.id", ondelete="CASCADE"),
                       primary_key=True)

    __table_args__ = (
        Index("IX_teacher_courses_course_id", "course_id"),
    )
