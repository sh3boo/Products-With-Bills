using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NationalTask.Models;

namespace NationalTask.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Bill>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Bills)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BillDetail>()
                .HasOne(bd => bd.Bill)
                .WithMany(b => b.BillDetails)
                .HasForeignKey(bd => bd.BillId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BillDetail>()
                .HasOne(bd => bd.Product)
                .WithMany(p => p.BillDetails)
                .HasForeignKey(bd => bd.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "tea", Price = 10.00m },
                new Product { Id = 2, Name = "sugar", Price = 25.00m },
                new Product { Id = 3, Name = "salad", Price = 20.00m },
                new Product { Id = 4, Name = "coffee", Price = 30.00m },
                new Product { Id = 5, Name = "Iced Coffee", Price = 80.00m }
            );
        }
    }
} 