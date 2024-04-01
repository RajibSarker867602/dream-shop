using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace DreamShop.Services.ProductAPI.Models
{
    public class Product
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        [Range(1, 1000)]
        public double Price { get; set; }
        public string ImageUrl { get; set; }

    }
}
