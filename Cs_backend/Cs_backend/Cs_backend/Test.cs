using Microsoft.EntityFrameworkCore;
using Xunit;
using Cs_backend.Database;
using Cs_backend.Models;
using Xunit.Abstractions;
using Task = Cs_backend.Models.Task;
using TaskStatus = Cs_backend.Models.TaskStatus;

namespace Cs_backend.Tests
{
    public class ApplicationContextTests(ITestOutputHelper testOutputHelper)
    {
        const string connectionString = "User ID=postgres;Password=postgres;Host=localhost;Database=project";

        [Fact]
        public void ConnectionTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseNpgsql(connectionString)
                .Options;

            using var context = new ApplicationContext(options);
            try
            {
                // Выполним простой SQL-запрос, проверяющий соединение
                context.Database.OpenConnection();
                context.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Не удалось подключиться к базе данных: {ex.Message}");
            }
        }

        [Fact]
        public void TestToFill()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseNpgsql(connectionString)
                .Options;

            using var context = new ApplicationContext(options);
            context.Database.OpenConnection();
            var group1 = new Group()
            {
                Name = "ФТ-203-1"
            };
            context.Groups.Add(group1);
            var group2 = new Group()
            {
                Name = "ФТ-203-2"
            };
            context.Groups.Add(group2);
            var course1 = new Course()
            {
                Name = "ФИИТ 2024 осень",
                PasswordHash = 1224
            };
            context.Courses.Add(course1);
            var course2 = new Course()
            {
                Name = "ФИИТ 2025 весна",
                PasswordHash = 1225
            };
            context.Courses.Add(course2);
            var teacher = new Teacher()
            {
                Login = "volkova@gmail.com",
                PasswordHash = "$2a$11$idMtnmRVsoB6EZjfe6Crk.aZpW949JN9c0gegB2IkjQpr4pKeJAVK",
                Name = "Саша",
            };

            context.Teachers.Add(teacher);
            context.SaveChanges();
            context.TeacherCourses.Add(new TeacherCourse()
            {
                Teacher = teacher,
                Course = course1,
            });
            context.TeacherCourses.Add(new TeacherCourse()
            {
                Teacher = teacher,
                Course = course2,
            });
            var student1 = new Student()
            {
                GroupId = group1.Id,
                Name = "Эдик Рашитов",
                TelegramId = 123,
                CourseId = course1.Id,
            };
            var student2 = new Student()
            {
                GroupId = group1.Id,
                Name = "Миша Зюков",
                TelegramId = 1234,
                CourseId = course2.Id,
            };
            var student3 = new Student()
            {
                GroupId = group2.Id,
                Name = "Антон Жданов",
                TelegramId = 1235,
                CourseId = course1.Id,
            };
            var student4 = new Student()
            {
                GroupId = group2.Id,
                Name = "Степан Гребнев",
                TelegramId = 1236,
                CourseId = course2.Id,
            };
            context.Students.Add(student1);
            context.Students.Add(student2);
            context.Students.Add(student3);
            context.Students.Add(student4);
            context.SaveChanges();
            context.Statuses.Add(new TaskStatus()
            {
                Name = "Aboba"
            });
            var task1 = new Task()
            {
                Topic = "Тервер 1",
                TaskLink = @"1\1\1",
                Deadline = DateTime.Now,
                TeacherId = teacher.Id,
                IsGrave = false,
                CourseId = course1.Id,
            };
            var task2 = new Task()
            {
                Topic = "Матстат 1",
                TaskLink = @"2\1\1",
                Deadline = DateTime.Now,
                TeacherId = teacher.Id,
                IsGrave = false,
                CourseId = course2.Id,
            };
            context.Tasks.Add(task1);
            context.Tasks.Add(task2);
            context.SaveChanges();
            context.SubmittedTasks.Add(new SubmittedTask()
            {
                StudentId = student1.Id,
                TaskId = task1.Id,
                StatusId = 1,
                HomeworkPrefix = @"1/1/1",
                SubmittedDate = DateTime.Now,
                Grade = 0,
                Comment = "Abugaga"
            });
            context.SubmittedTasks.Add(new SubmittedTask()
            {
                StudentId = student2.Id,
                TaskId = task2.Id,
                StatusId = 1,
                HomeworkPrefix = @"2/1/1",
                SubmittedDate = DateTime.Now,
                Grade = 0,
                Comment = "Abugaga"
            });
            context.SaveChanges();
        }
    }
}