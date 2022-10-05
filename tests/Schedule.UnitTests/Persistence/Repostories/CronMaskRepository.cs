namespace Schedule.UnitTests;

public class CronMaskRepository_Tests
{
    [Fact]
    public async Task GetIdUnixTimestampMap_MustBeValid()
    {
        // arrange
        var options = new DbContextOptionsBuilder<ScheduleContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var crons = new List<CronMask>()
        {
            new CronMask() { Id = 1, Mask = "0 0 1 * *" },
            new CronMask() { Id = 2, Mask = "0 0 5 * *" }
        };
        using var context = new ScheduleContext(options);
        await context.CronMasks.AddRangeAsync(crons);
        await context.SaveChangesAsync();
        var repo = new CronMaskRepository(context, NullLogger<CronMaskRepository>.Instance);

        // act
        var result = await repo.GetIdUnixTimestampMap(new DateTime(2022, 10, 5, 0, 0, 0, DateTimeKind.Utc), default);

        // assert
        Assert.Equal(crons.Select(x => x.Id), result.Keys);
        Assert.Equal(new int[] { 1664582400, 1664928000 }, result.Values);
    }
}