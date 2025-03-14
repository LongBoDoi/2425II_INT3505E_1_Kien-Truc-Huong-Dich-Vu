using Microsoft.AspNetCore.Mvc;
using PastebinBackend.Common;
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
        public IActionResult CreatePaste(string content, EnumPasteExposure exposure, string? pasteName, DateTime? expiresAt)
        {
            var paste = new Paste
            {
                Content = content,
                PasteName = pasteName ?? String.Empty,
                ExpiresAt = expiresAt,
                Exposure = exposure
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
        public IActionResult GetPasteData(string pasteKey)
        {
            try
            {
                Paste? paste = _context.Pastes.FirstOrDefault(p => p.PasteKey == pasteKey);
                if (paste != null)
                {
                    //Analytic? analytic = _context.Analytics.FirstOrDefault(a => a.PasteId == paste.Id && a.ViewDate.Date == DateTime.UtcNow.Date);
                    //if (analytic != null)
                    //{
                    //    analytic.ViewsCount++;
                    //}
                    //else
                    //{
                    //    analytic = new Analytic
                    //    {
                    //        PasteId = paste.Id,
                    //        ViewDate = DateTime.UtcNow,
                    //        ViewsCount = 1
                    //    };
                    //    _context.Analytics.Add(analytic);
                    //}
                    paste.Views++;

                    if (_context.SaveChanges() > 0)
                    {
                        return Content($"content={paste.Content};createdAt={paste.CreatedAt:yyyy-MM-dd HH:mm:ss};views={paste.Views};pasteName={paste.PasteName}");
                    }
                }

                return Content("Không tìm thấy mã paste");
            } catch (Exception e)
            {
                return Content($"Có lỗi xảy ra: {e.Message}");
            }
        }

        /// <summary>
        /// Lấy nội dung văn bản từ paste
        /// </summary>
        /// <param name="pasteKey"></param>
        /// <returns></returns>
        public IActionResult GetRecentPastes()
        {
            try
            {
                List<Paste> pastes = _context.Pastes.Where(p => p.Exposure == EnumPasteExposure.Public).OrderByDescending(p => p.CreatedAt).Take(10).ToList();
                if (pastes.Any())
                {
                    return Content(String.Join("|", pastes.Select(p => $"pasteKey={p.PasteKey};createdAt={p.CreatedAt:yyyy-MM-dd HH:mm:ss};pasteName={p.PasteName}")));
                }

                return Content("Không có dữ liệu");
            }
            catch (Exception e)
            {
                return Content($"Có lỗi xảy ra: {e.Message}");
            }
        }

        /// <summary>
        /// Cập nhật mã paste
        /// </summary>
        /// <param name="content">Nội dung văn bản</param>
        /// <param name="expiresAt">Thời gian hết hạn</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdatePaste(string pasteKey, string content, EnumPasteExposure exposure, string? pasteName, DateTime? expiresAt)
        {
            try
            {
                Paste? paste = _context.Pastes.FirstOrDefault(p => p.PasteKey == pasteKey);
                if (paste != null)
                {
                    paste.Content = content;
                    paste.PasteName = pasteName ?? String.Empty;
                    paste.ExpiresAt = expiresAt;
                    paste.Exposure = exposure;

                    _context.Entry(paste).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    if (_context.SaveChanges() > 0)
                    {
                        return Content("Cập nhật thành công");
                    }
                }

                return Content("Không tìm thấy mã paste");
            }
            catch (Exception e)
            {
                return Content($"Có lỗi xảy ra: {e.Message}");
            }
        }

        /// <summary>
        /// Xoá mã paste
        /// </summary>
        /// <param name="content">Nội dung văn bản</param>
        /// <param name="expiresAt">Thời gian hết hạn</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeletePaste(string pasteKey)
        {
            try
            {
                Paste? paste = _context.Pastes.FirstOrDefault(p => p.PasteKey == pasteKey);
                if (paste != null)
                {
                    _context.Entry(paste).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

                    if (_context.SaveChanges() > 0)
                    {
                        return Content("Xoá thành công");
                    }
                }

                return Content("Không tìm thấy mã paste");
            }
            catch (Exception e)
            {
                return Content($"Có lỗi xảy ra: {e.Message}");
            }
        }
    }
}
