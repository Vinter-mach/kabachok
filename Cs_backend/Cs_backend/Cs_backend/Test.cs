using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Collections.Generic;
using Cs_backend.Database;
using Cs_backend.Models;
using Xunit.Abstractions;
using Task = Cs_backend.Models.Task;
using TaskStatus = Cs_backend.Models.TaskStatus;

public class ApplicationContextTests
{
    private readonly ITestOutputHelper testOutputHelper;

    public ApplicationContextTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

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
        try
        {
            // Выполним простой SQL-запрос, проверяющий соединение
            context.Database.OpenConnection();
            context.Groups.Add(new Group()
            {
                Id = 1,
                Name = "ФТ-203-1"
            });
            context.Groups.Add(new Group()
            {
                Id = 2,
                Name = "ФТ-203-2"
            });
            context.Courses.Add(new Course()
            {
                Id = 1,
                Name = "ФИИТ 2024 осень",
                PasswordHash = 1224
            });
            context.Courses.Add(new Course()
            {
                Id = 2,
                Name = "ФИИТ 2025 весна",
                PasswordHash = 1225
            });
            context.TeacherCourses.Add(new TeacherCourse()
            {
                TeacherId = 1,
                CourseId = 1,
            });
            context.TeacherCourses.Add(new TeacherCourse()
            {
                TeacherId = 2,
                CourseId = 1,
            });
            context.TeacherCourses.Add(new TeacherCourse()
            {
                TeacherId = 3,
                CourseId = 2,
            });
            context.TeacherCourses.Add(new TeacherCourse()
            {
                TeacherId = 4,
                CourseId = 2,
            });
            context.Students.Add(new Student()
            {
                Id = 1,
                GroupId = 1,
                Name = "Эдик Рашитов",
                TelegramId = 123,
                CourseId = 1,
            });
            context.Students.Add(new Student()
            {
                Id = 2,
                GroupId = 1,
                Name = "Миша Зюков",
                TelegramId = 1234,
                CourseId = 2,
            });
            context.Students.Add(new Student()
            {
                Id = 3,
                GroupId = 2,
                Name = "Антон Жданов",
                TelegramId = 1235,
                CourseId = 1,
            });
            context.Students.Add(new Student()
            {
                Id = 4,
                GroupId = 2,
                Name = "Степан Гребнев",
                TelegramId = 1236,
                CourseId = 2,
            });
            context.Statuses.Add(new TaskStatus()
            {
                Id = 1,
                Name = "Aboba"
            });
            context.Tasks.Add(new Task()
            {
                Id = 1,
                Topic = "Тервер 1",
                TaskLink = @"1\1\1",
                Deadline = DateOnly.MaxValue,
                TeacherId = 1,
                IsGrave = false
            });
            context.Tasks.Add(new Task()
            {
                Id = 1,
                Topic = "Тервер 1",
                TaskLink = @"1\1\1",
                Deadline = DateOnly.MaxValue,
                TeacherId = 1,
                IsGrave = false
            });
            context.Tasks.Add(new Task()
            {
                Id = 1,
                Topic = "Тервер 1",
                TaskLink = @"1\1\1",
                Deadline = DateOnly.MaxValue,
                TeacherId = 1,
                IsGrave = false
            });
            context.Tasks.Add(new Task()
            {
                Id = 1,
                Topic = "Тервер 1",
                TaskLink = @"1\1\1",
                Deadline = DateOnly.MaxValue,
                TeacherId = 1,
                IsGrave = false
            });
            context.Database.CloseConnection();
        }
        catch (Exception ex)
        {
            Assert.Fail($"Не удалось подключиться к базе данных: {ex.Message}");
        }
    }

    [Fact]
    public void EnvironmentKeyTest()
    {
        if (Environment.GetEnvironmentVariable("MY_SECRET_KEY") == null)
        {
            DotNetEnv.Env.Load();
        }
        Assert.Equal("abc123supersecretkey", Environment.GetEnvironmentVariable("MY_SECRET_KEY"));
    }
}