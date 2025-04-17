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
        // Выполним простой SQL-запрос, проверяющий соединение
        context.Database.OpenConnection();
        context.Groups.Add(new Group()
        {
            Name = "ФТ-203-1"
        });
        context.Groups.Add(new Group()
        {
            Name = "ФТ-203-2"
        });
        context.Courses.Add(new Course()
        {
            Name = "ФИИТ 2024 осень",
            PasswordHash = 1224
        });
        context.Courses.Add(new Course()
        {
            Name = "ФИИТ 2025 весна",
            PasswordHash = 1225
        });
        context.Teachers.Add(new Teacher()
        {
            Login = "volkova@gmail.com",
            PasswordHash = "$2a$11$idMtnmRVsoB6EZjfe6Crk.aZpW949JN9c0gegB2IkjQpr4pKeJAVK",
            Name = "Саша",
        });
        context.TeacherCourses.Add(new TeacherCourse()
        {
            TeacherId = 1,
            CourseId = 1,
        });
        context.TeacherCourses.Add(new TeacherCourse()
        {
            TeacherId = 1,
            CourseId = 2,
        });
        context.Students.Add(new Student()
        {
            GroupId = 1,
            Name = "Эдик Рашитов",
            TelegramId = 123,
            CourseId = 1,
        });
        context.Students.Add(new Student()
        {
            GroupId = 1,
            Name = "Миша Зюков",
            TelegramId = 1234,
            CourseId = 2,
        });
        context.Students.Add(new Student()
        {
            GroupId = 2,
            Name = "Антон Жданов",
            TelegramId = 1235,
            CourseId = 1,
        });
        context.Students.Add(new Student()
        {
            GroupId = 2,
            Name = "Степан Гребнев",
            TelegramId = 1236,
            CourseId = 2,
        });
        context.Statuses.Add(new TaskStatus()
        {
            Name = "Aboba"
        });
        context.Tasks.Add(new Task()
        {
            Topic = "Тервер 1",
            TaskLink = @"1\1\1",
            Deadline = DateOnly.MaxValue,
            TeacherId = 1,
            IsGrave = false,
            CourseId = 1,
        });
        context.Tasks.Add(new Task()
        {
            Topic = "Матстат 1",
            TaskLink = @"2\1\1",
            Deadline = DateOnly.MaxValue,
            TeacherId = 1,
            IsGrave = false,
            CourseId = 2,
        });
        context.SaveChanges();
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