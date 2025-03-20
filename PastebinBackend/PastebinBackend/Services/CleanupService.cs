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
    private readonly string _logPath = Path.Combine(Directory.GetCurrentDirectory(), "Log");
    private readonly string _logFile = "CleanupLog.txt";

    public CleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        EnsureLogFileExists();
        WriteLog("CleanupService initialized.");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            WriteLog("Cleanup started");
            try
            {
                // Tính thời gian còn lại tới nửa đêm
                TimeSpan timeUntilMidnight = GetTimeUntilMidnight();

                // await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                await Task.Delay(timeUntilMidnight, stoppingToken); // Chờ task đến 00:00

                WriteLog("Running cleanup...");

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
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog($"Có lỗi xảy ra: {ex.Message}");
            }
        }
    }

    private TimeSpan GetTimeUntilMidnight()
    {
        DateTime now = DateTime.UtcNow;
        DateTime nextMidnight = now.Date.AddDays(1);
        return nextMidnight - now;
    }

    /// <summary>
    /// Kiểm tra file log tồn tại
    /// </summary>
    private void EnsureLogFileExists()
    {
        try
        {
            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }

            if (!File.Exists(Path.Combine(_logPath, _logFile)))
            {
                File.Create(Path.Combine(_logPath, _logFile)).Dispose(); // Create and close the file
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating log file: {ex.Message}");
        }
    }

    // Helper method to write logs safely
    private void WriteLog(string message)
    {
        try
        {
            File.AppendAllText(Path.Combine(_logPath, _logFile), $"[{DateTime.UtcNow}]: {message}\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }
}
