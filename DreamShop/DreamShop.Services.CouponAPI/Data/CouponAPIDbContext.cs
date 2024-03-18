using DreamShop.Services.CouponAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DreamShop.Services.CouponAPI.Data
{
    public class CouponAPIDbContext: DbContext
    {
        public CouponAPIDbContext(DbContextOptions<CouponAPIDbContext> options):base(options)
        {
            
        }

        // db sets
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                Id = 1,
                CouponCode = "A",
                DiscountAmount = 10,
                MinAmount = 10
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon()
            {
                Id = 2,
                CouponCode = "B",
                DiscountAmount = 30,
                MinAmount = 15
            });
        }
    }
}
