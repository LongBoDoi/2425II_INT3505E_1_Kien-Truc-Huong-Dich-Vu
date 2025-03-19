using Microsoft.EntityFrameworkCore;
using PastebinBackend.Models;

namespace PastebinBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Paste> Pastes { get; set; }
        public DbSet<Analytic> Analytics { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Gán unique cho paste key của bảng Paste
            modelBuilder.Entity<Paste>()
                .HasIndex(p => p.PasteKey)
                .IsUnique();

            //// Foreign key bảng Paste - Analytic
            //modelBuilder.Entity<Analytic>()
            //    .HasOne(a => a.Paste)
            //    .WithMany()
            //    .HasForeignKey(a => a.PasteId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
