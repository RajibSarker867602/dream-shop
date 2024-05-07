using System.ComponentModel.DataAnnotations;

namespace DreamShop.Services.ShoppingCartAPI.Models.Dtos
{
    public class ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
