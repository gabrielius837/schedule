namespace Schedule.UnitTests;

public class ScheduleController_Tests
{
    [Fact]
    public async Task GetSchedule_MustFind()
    {
        // arrange
        var repository = new Mock<IScheduleRepository>();
        var id = Guid.NewGuid();
        var schedule = new ScheduleResponse(id, new string[] {"str1", "str2"});
        repository.Setup(x => x.GetSchedule(id, It.IsAny<DateTime>(), default(CancellationToken))).ReturnsAsync(schedule);
        var controller = new ScheduleController(NullLogger<ScheduleController>.Instance, repository.Object);

        // act
        var response = await controller.GetSchedule(id);
        var result = response as OkObjectResult;

        // assert
        Assert.NotNull(result);
        Assert.Equal(200, result?.StatusCode);
        Assert.Equal(schedule, result?.Value);
    }

    [Fact]
    public async Task GetSchedule_MustNotFind()
    {
        // arrange
        var repository = new Mock<IScheduleRepository>();
        var id = Guid.NewGuid();
        ScheduleResponse? schedule = null;
        repository.Setup(x => x.GetSchedule(id, It.IsAny<DateTime>(), default(CancellationToken))).ReturnsAsync(schedule);
        var controller = new ScheduleController(NullLogger<ScheduleController>.Instance, repository.Object);
        controller.ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() };

        // act
        var response = await controller.GetSchedule(id);
        var result = response as NotFoundObjectResult;


        // assert
        Assert.NotNull(result);
        Assert.Equal(404, result?.StatusCode);
        Assert.True(result?.Value is ErrorResponse);
    }
}