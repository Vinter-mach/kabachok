using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Collections.Generic;
using Cs_backend.Database;
using Cs_backend.Models;
using Xunit.Abstractions;

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
        using (var context = new ApplicationContext(connectionString))
        {
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
    }
}