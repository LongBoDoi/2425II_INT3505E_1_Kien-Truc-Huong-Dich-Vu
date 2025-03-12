using Microsoft.AspNetCore.Mvc;
using PastebinBackend.Data;
using PastebinBackend.Models;

namespace PastebinBackend.Controllers
{
    public class PasteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PasteController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Tạo mã paste
        /// </summary>
        /// <param name="content">Nội dung văn bản</param>
        /// <param name="expiresAt">Thời gian hết hạn</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreatePaste(string content, DateTime? expiresAt)
        {
            var paste = new Paste
            {
                Content = content,
                ExpiresAt = expiresAt
            };

            _context.Pastes.Add(paste);
            _context.SaveChanges();

            return Content(paste.PasteKey);
        }

        /// <summary>
        /// Lấy nội dung văn bản từ paste
        /// </summary>
        /// <param name="pasteKey"></param>
        /// <returns></returns>
        public IActionResult GetPasteContent(string pasteKey)
        {
            try
            {
                Paste? paste = _context.Pastes.FirstOrDefault(p => p.PasteKey == pasteKey);
                if (paste != null)
                {
                    Analytic? analytic = _context.Analytics.FirstOrDefault(a => a.PasteId == paste.Id && a.ViewDate.Date == DateTime.UtcNow.Date);
                    if (analytic != null)
                    {
                        analytic.ViewsCount++;
                    }
                    else
                    {
                        analytic = new Analytic
                        {
                            PasteId = paste.Id,
                            ViewDate = DateTime.UtcNow,
                            ViewsCount = 1
                        };
                        _context.Analytics.Add(analytic);
                    }
                    paste.Views++;

                    if (_context.SaveChanges() > 0)
                    {
                        return Content(paste.Content, "text/plain");
                    }
                }

                return Content("Không tìm thấy mã paste");
            } catch (Exception e)
            {
                return Content($"Có lỗi xảy ra: {e.Message}");
            }
        }
    }
}
