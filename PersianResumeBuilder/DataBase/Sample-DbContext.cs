using Microsoft.EntityFrameworkCore;
using PersianResumeBuilder.Entities;

namespace PersianResumeBuilder.DataBase
{
    public class Sample_DbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-OVR7TD7\\;Database=Resume_DataBase;TrustServerCertificate=True;Integrated Security=true;");
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<InformationCustomerProfile> informationCustomerProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .Property(c => c.Price)
                .HasPrecision(18, 4);
        }

    }
}
