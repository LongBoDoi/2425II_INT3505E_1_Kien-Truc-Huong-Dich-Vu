using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PastebinBackend.Models
{
    public class Analytic
    {
        /// <summary>
        /// Khoá chính
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Khoá ngoại
        /// ID của bảng Paste
        /// </summary>
        [Required]
        public long PasteId { get; set; }

        /// <summary>
        /// Object paste
        /// </summary>
        [ForeignKey("PasteId")]
        public Paste? Paste { get; set; }

        /// <summary>
        /// Ngày xem
        /// </summary>
        public DateTime ViewDate { get; set; }

        /// <summary>
        /// Số lượng xem trong ngày
        /// </summary>
        public int ViewsCount { get; set; } = 0;
    }
}
