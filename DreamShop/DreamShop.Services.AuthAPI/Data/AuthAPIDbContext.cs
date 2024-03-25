using DreamShop.Services.CouponAPI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DreamShop.Services.AuthAPI.Data
{
    public class AuthAPIDbContext: IdentityDbContext<ApplicationUser>
    {
        public AuthAPIDbContext(DbContextOptions<AuthAPIDbContext> options):base(options)
        {
            
        }

        // db sets

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
