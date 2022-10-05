namespace Schedule.UnitTests;

public class DateTimeExtensions_Tests
{
    [Fact]
    public void ToMonthDateTimeRange_MustBeValid()
    {
        // arrange
        var date = DateTime.UtcNow;

        // act
        var (lower, upper) = date.ToMonthDateTimeRange();

        // assert
        Assert.Equal(date.Month, lower.AddTicks(1).Month);
        Assert.Equal(date.Month, upper.AddTicks(-1).Month);
    }

    [Fact]
    public void ToMonthUnixTimestampRange_MustBeValid()
    {
        // arrange
        var input = new DateTime(2022, 10, 4, 0, 0, 0, DateTimeKind.Utc);
        var expectedLower = 1664582400;
        var expectedUpper = 1667260800;

        // act
        var (lower, upper) = input.ToMonthUnixTimestampRange();

        // assert
        Assert.Equal(expectedLower, lower);
        Assert.Equal(expectedUpper, upper);
    }

    [Fact]
    public void ToFormattedDate_MustMatchExpectedValue()
    {
        // arrange
        var input = 1664798417;
        var expected = "03/10/2022";

        // act
        var result = input.ToFormattedDate();

        // assert
        Assert.Equal(expected, result);
    }
}