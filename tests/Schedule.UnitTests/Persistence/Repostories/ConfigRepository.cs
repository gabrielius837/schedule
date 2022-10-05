namespace Schedule.UnitTests;

public class ConfigRepository_Tests
{
    private static readonly CompanyType[] types = new CompanyType[]
    {
        new CompanyType() { Id = 1, Name = "all" },
        new CompanyType() { Id = 2, Name = "small" },
        new CompanyType() { Id = 3, Name = "medium" },
        new CompanyType() { Id = 4, Name = "large" }
    };

    [Fact]
    public async Task GetConfigFull_MustFetchAll()
    {
        // arrange
        var options = new DbContextOptionsBuilder<ScheduleContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var context = new ScheduleContext(options);
        await context.CompanyTypes.AddRangeAsync(types);
        var configs = new Config[]
        {
            new Config() { CronMaskId = 1, MarketId = 1, CompanyTypeId = 1 },
            new Config() { CronMaskId = 2, MarketId = 2, CompanyTypeId = 2 }
        };
        await context.Configs.AddRangeAsync(configs);
        await context.SaveChangesAsync();
        var repo = new ConfigRepository(context, NullLogger<ConfigRepository>.Instance);

        // act
        var result = await repo.GetConfigs(default);
        // 2 2 3 4 type
        // 1 2 1 1 market
        // 1 2 1 1 company type
        var ordered = result.OrderBy(x => x.CompanyTypeId).ThenBy(x => x.MarketId).ToArray();

        // assert
        Assert.Equal(4, result.Length);
        Assert.Equal(new int[] { 2, 2, 3, 4 }, ordered.Select(x => x.CompanyTypeId));
        Assert.Equal(new int[] { 1, 2, 1, 1 }, ordered.Select(x => x.MarketId));
        Assert.Equal(new int[] { 1, 2, 1, 1 }, ordered.Select(x => x.CronMaskId));
    }

    [Fact]
    public async Task GetConfigFull_MustFetchAll_WithoutDuplicate()
    {
        // arrange
        var options = new DbContextOptionsBuilder<ScheduleContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var context = new ScheduleContext(options);
        await context.CompanyTypes.AddRangeAsync(types);
        var configs = new Config[]
        {
            new Config() { CronMaskId = 1, MarketId = 1, CompanyTypeId = 1 },
            // duplicate
            new Config() { CronMaskId = 1, MarketId = 1, CompanyTypeId = 2 },
            new Config() { CronMaskId = 2, MarketId = 2, CompanyTypeId = 2 }
        };
        await context.Configs.AddRangeAsync(configs);
        await context.SaveChangesAsync();
        var repo = new ConfigRepository(context, NullLogger<ConfigRepository>.Instance);

        // act
        var result = await repo.GetConfigs(default);
        // 2 2 3 4 type
        // 1 2 1 1 market
        // 1 2 1 1 
        var ordered = result.OrderBy(x => x.CompanyTypeId).ThenBy(x => x.MarketId).ToArray();

        // assert
        Assert.Equal(4, result.Length);
        Assert.Equal(new int[] { 2, 2, 3, 4 }, ordered.Select(x => x.CompanyTypeId));
        Assert.Equal(new int[] { 1, 2, 1, 1 }, ordered.Select(x => x.MarketId));
        Assert.Equal(new int[] { 1, 2, 1, 1 }, ordered.Select(x => x.CronMaskId));
    }
}