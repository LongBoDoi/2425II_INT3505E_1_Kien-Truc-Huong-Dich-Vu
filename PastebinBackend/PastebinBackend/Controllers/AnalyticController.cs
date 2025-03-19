using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PastebinBackend.Data;
using PastebinBackend.Models;

namespace PastebinBackend.Controllers
{
    public class AnalyticController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalyticController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Tăng số view cho trang
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddViewAnalytic()
        {
            try
            {
                var now = DateTime.UtcNow;

                Analytic? existedRecord = _context.Analytics.FirstOrDefault(a => a.ViewDate.Date == now.Date);
                if (existedRecord != null)
                {
                    existedRecord.ViewCount++;
                    _context.Entry(existedRecord).State = EntityState.Modified;
                } else
                {
                    existedRecord = new Analytic
                    {
                        ViewDate = now,
                        ViewCount = 1
                    };
                    _context.Analytics.Add(existedRecord);
                }

                if (_context.SaveChanges() > 0)
                {
                    return Content("Tăng lượng view thành công");
                }

                return Content("Lỗi khi lưu bản ghi vào database");
            }
            catch (Exception e)
            {
                return Content($"Có lỗi xảy ra: {e.Message}");
            }
        }

        /// <summary>
        /// Lấy số view trang web
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetViewAnalyticByDay()
        {
            try
            {
                return Content(String.Join("|", _context.Analytics
                    .OrderByDescending(a => a.ViewDate)
                    .Select(a => $"time={a.ViewDate:yyyy-MM-dd};views={a.ViewCount}")
                    .ToList()
                ));
            }
            catch (Exception e)
            {
                return Content($"Có lỗi xảy ra: {e.Message}");
            }
        }

        /// <summary>
        /// Lấy số view trang web
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetViewAnalyticByMonth()
        {
            try
            {
                return Content(String.Join("|", _context.Analytics
                    .GroupBy(a => new { a.ViewDate.Month, a.ViewDate.Year})
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        ViewCount = g.Count()
                    })
                    .OrderByDescending(g => g.Year).ThenByDescending(g => g.Month)
                    .Select(g => $"time={g.Year}-{g.Month};views={g.ViewCount}") 
                    .ToList()
                ));
            }
            catch (Exception e)
            {
                return Content($"Có lỗi xảy ra: {e.Message}");
            }
        }

        /// <summary>
        /// Tăng số view cho trang
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult FakeAnalyticData()
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    DateTime start = new DateTime(2024, 1, 1, 0, 0, 0);
                    DateTime end = DateTime.UtcNow;

                    Random random = new Random();
                    TimeSpan range = end - start;

                    TimeSpan randomTimeSpan = new TimeSpan((long)(random.NextDouble() * range.Ticks));

                    DateTime randomDate = start + randomTimeSpan;

                    Analytic? existedRecord = _context.Analytics.FirstOrDefault(a => a.ViewDate.Date == randomDate.Date);
                    if (existedRecord != null)
                    {
                        existedRecord.ViewCount++;
                        _context.Entry(existedRecord).State = EntityState.Modified;
                    }
                    else
                    {
                        existedRecord = new Analytic
                        {
                            ViewDate = randomDate,
                            ViewCount = 1
                        };
                        _context.Analytics.Add(existedRecord);
                    }
                }

                if (_context.SaveChanges() > 0)
                {
                    return Content("Fake data Analytic thành công");
                }

                return Content("Lỗi khi lưu bản ghi vào database");
            }
            catch (Exception e)
            {
                return Content($"Có lỗi xảy ra: {e.Message}");
            }
        }
    }
}
