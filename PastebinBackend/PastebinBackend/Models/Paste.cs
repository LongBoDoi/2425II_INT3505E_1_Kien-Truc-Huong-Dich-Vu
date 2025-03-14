using PastebinBackend.Common;
using System.ComponentModel.DataAnnotations;

namespace PastebinBackend.Models
{
    public class Paste
    {
        /// <summary>
        /// Khoá chính
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Mã paste
        /// </summary>
        [Required]
        [StringLength(16)]
        public string PasteKey { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 16);

        /// <summary>
        /// Mã paste
        /// </summary>
        [StringLength(255)]
        public string PasteName { get; set; } = String.Empty;

        /// <summary>
        /// Nội dung văn bản
        /// </summary>
        [Required]
        public string Content { get; set; } = String.Empty;

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Thời gian hết hạn
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// Số lần xem
        /// </summary>
        public int Views { get; set; } = 0;

        /// <summary>
        /// Quyền riêng tư mã paste
        /// </summary>
        public EnumPasteExposure Exposure { get; set; }
    }
}
