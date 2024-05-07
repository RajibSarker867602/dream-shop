using DreamShop.Services.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamShop.Services.ShoppingCartAPI.Data
{
    public class ShoppingCartAPIDbContext: DbContext
    {
        public ShoppingCartAPIDbContext(DbContextOptions<ShoppingCartAPIDbContext> options):base(options)
        {
            
        }

        // db sets
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }

    }
}
