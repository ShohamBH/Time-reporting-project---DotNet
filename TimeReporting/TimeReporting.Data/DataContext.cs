using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TimeReporting.Core.Entities;

namespace TimeReporting.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<WorkLog> WorkLogs { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "329",
                    FirstName = "Shoham",
                    LastName = "Dahan",
                    Email = "shoham@example.com",
                    Phone = "0548502051",
                    City = "Tel Aviv",
                    Birthdate = new DateTime(2000, 1, 1),
                    Password = "123456",
                    Role = Role.Employee
                });

            modelBuilder.Entity<WorkLog>().HasData(
                new WorkLog
                {
                    Id = 1,
                    UserId = "329",
                    EntryTime = new DateTime(2025, 4, 10, 9, 0, 0),
                    ExitTime = new DateTime(2025, 4, 10, 17, 0, 0)
                });

            modelBuilder.Entity<LeaveRequest>().HasData(
                new LeaveRequest
                {
                    Id = 1,
                    UserId = "329", // שימו לב: שונה ל-"329" (string)
                    Type = LeaveType.Vacation, // הוספתי Type לדוגמה
                    StartDate = new DateTime(2025, 5, 1),
                    EndDate = new DateTime(2025, 5, 3),
                    Status = LeaveStatus.Pending
                }
            );
        }
    }
}