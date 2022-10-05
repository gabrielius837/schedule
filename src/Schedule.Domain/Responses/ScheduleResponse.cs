namespace Schedule.Domain;

public class ScheduleResponse
{
    public ScheduleResponse(Guid companyId, string[] notifications)
    {
        CompanyId = companyId;
        Notifications = notifications;
    }

    public Guid CompanyId { get; }
    public string[] Notifications { get; }
}
