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

                Analytic? existedRecord = _context.Analytics.FirstOrDefault(a => $"{a.ViewDate:yyyy-MM}" == $"{now:yyyy-MM}");
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
        public IActionResult GetViewAnalytic()
        {
            try
            {
                return Content(String.Join("|", _context.Analytics
                    .OrderByDescending(a => a.ViewDate)
                    .Select(a => $"time={a.ViewDate:yyyy-MM};views={a.ViewCount}")
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
