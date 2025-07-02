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
        public DbSet<PatientHistory> PatientHistories { get; set; }
        public DbSet<UserHistory> UserHistories { get; set; }
        public DbSet<BloodRequest> BloodRequests { get; set; }
        public DbSet<BloodRequestHistory> BloodRequestHistories{get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonorHistory>().HasKey(h => h.Id); // Explicitly set the primary key
            modelBuilder.Entity<PatientHistory>().HasKey(h=>h.Id);
            modelBuilder.Entity<UserHistory>().HasKey(h=>h.Id);
            // modelBuilder.Entity<BloodRequest>().HasKey(x => x.Id);
            base.OnModelCreating(modelBuilder);

        }
    }
}
