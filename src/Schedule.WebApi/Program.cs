using Schedule.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ScheduleContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddTransient<IConfigRepository, ConfigRepository>();
builder.Services.AddTransient<ICronMaskRepository, CronMaskRepository>();
builder.Services.AddTransient<INotificationRepository, NotificationRepository>();
builder.Services.AddTransient<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScheduler();
builder.Services.AddTransient<NotificationTask>();

var app = builder.Build();

// perform notification sync before starting
using (var provider = builder.Services.BuildServiceProvider())
{
    var logger = provider.GetService<ILogger>();
    var service = provider.GetService<INotificationRepository>();
    if (service is not null && logger is not null)
    {
        var seed = DateTime.UtcNow;
        logger.LogInformation("Synching notifications before launching with seed: {seed}", seed.ToString());
        var result = await service.WriteNotifications(seed, default);
    }
    else
        throw new Exception("Could not sync notifications, shutting down");
}

// of course it could reallocated to seperate worker service
app.Services.UseScheduler(scheduler =>
{
    scheduler
        .Schedule<NotificationTask>()
        // run it on every 28th day of the month
        .Cron("0 0 28 * *");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseTraceId();
app.UseErrorHandling();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
