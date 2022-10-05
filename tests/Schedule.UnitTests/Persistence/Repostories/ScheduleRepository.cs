namespace Schedule.UnitTests;

public class ScheduleRepository_Tests
{
    [Fact]
    public async Task GetSchedule_MustBeNull()
    {
        // arrange
        var options = new DbContextOptionsBuilder<ScheduleContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var context = new ScheduleContext(options);
        var repo = new ScheduleRepository(context);

        // act
        var result = await repo.GetSchedule(Guid.NewGuid(), new DateTime(2022, 10, 5, 0, 0, 0, DateTimeKind.Utc), default);

        // assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetSchedule_CompanyExistsButNoSchedule()
    {
        // arrange
        var options = new DbContextOptionsBuilder<ScheduleContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var context = new ScheduleContext(options);
        var company = new Company() { Id = Guid.NewGuid(), Name = "test", Number = "0123456789", CompanyTypeId = 2, MarketId = 2 };
        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();
        var repo = new ScheduleRepository(context);

        // act
        var result = await repo.GetSchedule(company.Id, new DateTime(2022, 10, 5, 0, 0, 0, DateTimeKind.Utc), default);

        // assert
        Assert.NotNull(result);
        Assert.Equal(company.Id, result?.CompanyId);
        Assert.Empty(result?.Notifications);
    }

    [Fact]
    public async Task GetSchedule_CompanyExistsWithSchedule()
    {
        // arrange
        var options = new DbContextOptionsBuilder<ScheduleContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var context = new ScheduleContext(options);
        var company = new Company() { Id = Guid.NewGuid(), Name = "test", Number = "0123456789", CompanyTypeId = 2, MarketId = 2 };
        var notification = new Notification() { CompanyTypeId = 2, MarketId = 2, Timestamp = 1664960946 };
        await context.Companies.AddAsync(company);
        await context.Notifications.AddAsync(notification);
        await context.SaveChangesAsync();
        var repo = new ScheduleRepository(context);

        // act
        var result = await repo.GetSchedule(company.Id, new DateTime(2022, 10, 5, 0, 0, 0, DateTimeKind.Utc), default);

        // assert
        Assert.NotNull(result);
        Assert.Equal(company.Id, result?.CompanyId);
        Assert.Equal(1, result?.Notifications.Length);
        Assert.Equal("05/10/2022", result?.Notifications[0]);
    }
}