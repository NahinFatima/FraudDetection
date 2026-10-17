using Microsoft.EntityFrameworkCore;
using FraudDetection.Domain.Entities;

namespace FraudDetection.Infrastructure
{
    public class FraudDetectionDbContext : DbContext 
    {
        public FraudDetectionDbContext( DbContextOptions<FraudDetectionDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<FraudAlert> FraudAlerts => Set<FraudAlert>();
        public DbSet<OtpCode> OtpCodes => Set<OtpCode>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Cnic).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.MobileNumber).IsUnique();
            modelBuilder.Entity<Account>().HasIndex(a => a.AccountNumber).IsUnique();
            modelBuilder.Entity<Transaction>().HasIndex(t => new { t.AccountId, t.IdempotencyKey }).IsUnique();
            modelBuilder.Entity<Account>().Property(a => a.Balance).HasPrecision(18, 2);
            modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();
            modelBuilder.Entity<User>().Property(u => u.VerificationStatus).HasConversion<string>();
            modelBuilder.Entity<Transaction>().Property(t => t.Status).HasConversion<string>();

        }



    }
}
