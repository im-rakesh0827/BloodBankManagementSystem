using BloodBankManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BloodBankManagementSystem.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }  
        public DbSet<Donor> Donors { get; set; }
        public DbSet<DonorHistory> DonorHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonorHistory>()
            .HasKey(h => h.Id); // ✅ Explicitly set the primary key
            base.OnModelCreating(modelBuilder);
        }
    }
}
