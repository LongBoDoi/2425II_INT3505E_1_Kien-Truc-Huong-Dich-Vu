using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PastebinBackend.Data;
using PastebinBackend.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class FakeAnalyticDataService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public FakeAnalyticDataService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    DateTime start = new DateTime(2024, 1, 1, 0, 0, 0);
                    DateTime end = DateTime.UtcNow;

                    Random random = new Random();
                    TimeSpan range = end - start;

                    TimeSpan randomTimeSpan = new TimeSpan((long)(random.NextDouble() * range.Ticks));

                    DateTime randomDate = start + randomTimeSpan;

                    Analytic? existedRecord = dbContext.Analytics.FirstOrDefault(a => a.ViewDate.Date == randomDate.Date);
                    if (existedRecord != null)
                    {
                        existedRecord.ViewCount++;
                        dbContext.Entry(existedRecord).State = EntityState.Modified;
                    }
                    else
                    {
                        existedRecord = new Analytic
                        {
                            ViewDate = randomDate,
                            ViewCount = 1
                        };
                        dbContext.Analytics.Add(existedRecord);
                    }

                    if (dbContext.SaveChanges() > 0)
                    {
                        Console.WriteLine("Fake dữ liệu Analytic thành công");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Có lỗi xảy ra: {ex.Message}");
            }
        }
    }
}
