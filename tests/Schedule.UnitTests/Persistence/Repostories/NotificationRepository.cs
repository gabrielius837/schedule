namespace Schedule.UnitTests;

public class NotificationRepository_Tests
{
    [Fact]
    public async Task WriteNotifications_MustReturnExpectedCount()
    {
        // arrange
        var options = new DbContextOptionsBuilder<ScheduleContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var context = new ScheduleContext(options);
        CancellationToken token = default;
        var seed = new DateTime(2022, 10, 5, 0, 0, 0, DateTimeKind.Utc);
        var configRepo = new Mock<IConfigRepository>();
        var configs = new Config[]
        {
            new Config() { MarketId = 1, CronMaskId = 1, CompanyTypeId = 2 },
            new Config() { MarketId = 1, CronMaskId = 1, CompanyTypeId = 3 },
            new Config() { MarketId = 1, CronMaskId = 1, CompanyTypeId = 4 },
            new Config() { MarketId = 3, CronMaskId = 3, CompanyTypeId = 3 },
        };
        configRepo.Setup(x => x.GetConfigs(token)).ReturnsAsync(configs);
        var cronMaskRepository = new Mock<ICronMaskRepository>();
        IReadOnlyDictionary<int, int> crons = new Dictionary<int, int>()
        {
            { 1, 1664582400 },
            { 2, 1664928000 }
        };
        cronMaskRepository.Setup(x => x.GetIdUnixTimestampMap(seed, token)).ReturnsAsync(crons);
        var repo = new NotificationRepository(context, NullLogger<NotificationRepository>.Instance, configRepo.Object, cronMaskRepository.Object);

        // act
        var result = await repo.WriteNotifications(seed, token);
        var notifications = await context.Notifications.ToArrayAsync();

        // assert
        Assert.Equal(3, result);
        Assert.Equal(3, notifications.Length);
        Assert.NotNull(notifications.FirstOrDefault(x => x.MarketId == 1 && x.CompanyTypeId == 2 && x.Timestamp == 1664582400));
        Assert.NotNull(notifications.FirstOrDefault(x => x.MarketId == 1 && x.CompanyTypeId == 3 && x.Timestamp == 1664582400));
        Assert.NotNull(notifications.FirstOrDefault(x => x.MarketId == 1 && x.CompanyTypeId == 4 && x.Timestamp == 1664582400));
    }

    [Fact]
    public async Task WriteNotifications_MustReturnExpectedCount_WhenDuplicateExists()
    {
        // arrange
        var options = new DbContextOptionsBuilder<ScheduleContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var context = new ScheduleContext(options);
        var notification = new Notification() { MarketId = 1, CompanyTypeId = 2, Timestamp = 1664582400 };
        await context.Notifications.AddAsync(notification);
        await context.SaveChangesAsync();
        CancellationToken token = default;
        var seed = new DateTime(2022, 10, 5, 0, 0, 0, DateTimeKind.Utc);
        var configRepo = new Mock<IConfigRepository>();
        var configs = new Config[]
        {
            new Config() { MarketId = 1, CronMaskId = 1, CompanyTypeId = 2 },
            new Config() { MarketId = 1, CronMaskId = 1, CompanyTypeId = 3 },
            new Config() { MarketId = 1, CronMaskId = 1, CompanyTypeId = 4 },
            new Config() { MarketId = 3, CronMaskId = 3, CompanyTypeId = 3 },
        };
        configRepo.Setup(x => x.GetConfigs(token)).ReturnsAsync(configs);
        var cronMaskRepository = new Mock<ICronMaskRepository>();
        IReadOnlyDictionary<int, int> crons = new Dictionary<int, int>()
        {
            { 1, 1664582400 },
            { 2, 1664928000 }
        };
        cronMaskRepository.Setup(x => x.GetIdUnixTimestampMap(seed, token)).ReturnsAsync(crons);
        var repo = new NotificationRepository(context, NullLogger<NotificationRepository>.Instance, configRepo.Object, cronMaskRepository.Object);

        // act
        var result = await repo.WriteNotifications(seed, token);
        var notifications = await context.Notifications.ToArrayAsync();

        // assert
        // because duplicate is already in db
        Assert.Equal(2, result);
        Assert.Equal(3, notifications.Length);
        Assert.NotNull(notifications.FirstOrDefault(x => x.MarketId == 1 && x.CompanyTypeId == 2 && x.Timestamp == 1664582400));
        Assert.NotNull(notifications.FirstOrDefault(x => x.MarketId == 1 && x.CompanyTypeId == 3 && x.Timestamp == 1664582400));
        Assert.NotNull(notifications.FirstOrDefault(x => x.MarketId == 1 && x.CompanyTypeId == 4 && x.Timestamp == 1664582400));
    }
}