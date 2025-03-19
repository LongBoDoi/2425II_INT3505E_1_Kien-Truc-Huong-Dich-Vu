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
        /// Ngày xem
        /// </summary>
        public DateTime ViewDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Số lượt xem
        /// </summary>
        public int ViewCount { get; set; }
    }
}
