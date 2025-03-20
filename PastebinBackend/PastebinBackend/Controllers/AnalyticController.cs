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
        public IActionResult GetViewAnalyticByDay(string month)
        {
            try
            {
                if (string.IsNullOrEmpty(month) || !DateTime.TryParseExact(month + "-01", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedMonth))
                {
                    return Content("Vui lòng cung cấp tháng theo định dạng yyyy-MM");
                }

                var analyticsData = _context.Analytics
                    .Where(a => a.ViewDate.Year == parsedMonth.Year && a.ViewDate.Month == parsedMonth.Month)
                    .OrderByDescending(a => a.ViewDate.Date)
                    .Select(a => $"time={a.ViewDate:yyyy-MM-dd};views={a.ViewCount}") 
                    .ToList();

                if (!analyticsData.Any())
                {
                    return Content($"Không có dữ liệu cho tháng {month}");
                }

                return Content(string.Join("|", analyticsData)); // Nối các phần tử bằng "|"
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
                     .AsEnumerable()
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                         Month = g.Key.Month.ToString("D2"),
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
    }
}
