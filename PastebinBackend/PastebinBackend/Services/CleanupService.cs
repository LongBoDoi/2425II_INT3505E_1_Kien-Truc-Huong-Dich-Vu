using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PastebinBackend.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class CleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public CleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
	Console.WriteLine("Constructor called");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Tính thời gian còn lại tới nửa đêm
                TimeSpan timeUntilMidnight = GetTimeUntilMidnight();
                Console.WriteLine($"Thực thi task dọn dẹp trong {timeUntilMidnight}...");

                // await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                await Task.Delay(timeUntilMidnight, stoppingToken); // Chờ task đến 00:00

                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var now = DateTime.UtcNow;

                    var expiredPastes = await dbContext.Pastes
                        .Where(p => p.ExpiresAt != null && p.ExpiresAt <= now)
                        .ToListAsync();

                    if (expiredPastes.Any())
                    {
                        dbContext.Pastes.RemoveRange(expiredPastes);
                        await dbContext.SaveChangesAsync();
                        Console.WriteLine($"Đã dọn {expiredPastes.Count} mã paste hết hạn vào lúc {now}.");
                    }
                    else
                    {
                        Console.WriteLine($"Không có mã paste hết hạn vào lúc {now}.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Có lỗi xảy ra: {ex.Message}");
            }
        }
    }

    private TimeSpan GetTimeUntilMidnight()
    {
        DateTime now = DateTime.UtcNow;
        DateTime nextMidnight = now.Date.AddDays(1);
        return nextMidnight - now;
    }
}
