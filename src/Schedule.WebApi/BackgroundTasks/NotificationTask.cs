namespace Schedule.WebApi;

public class NotificationTask : IInvocable
{
    private readonly INotificationRepository _repository;
    private readonly ILogger<NotificationTask> _logger;

    public NotificationTask(INotificationRepository repository, ILogger<NotificationTask> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task Invoke()
    {
        try
        {
            _logger.LogInformation("Starting notification task");
            // generate notifications for next month;
            var seed = DateTime.UtcNow.AddMonths(1);
            var result = await _repository.WriteNotifications(seed, default);
            _logger.LogInformation("Successful notification task, {count} entries generated with seed: {seed}", result, seed.ToString());
        }
        catch (System.Exception ex)
        {
            _logger.LogError("Error occured during notification task: {error}", ex);
            throw ex;
        }
    }
}