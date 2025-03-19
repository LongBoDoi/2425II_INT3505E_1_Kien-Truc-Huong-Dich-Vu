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
                _context.Analytics.Add(new Analytic());

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
                return Content(_context.Analytics.Count().ToString());
            }
            catch (Exception e)
            {
                return Content($"Có lỗi xảy ra: {e.Message}");
            }
        }
    }
}
