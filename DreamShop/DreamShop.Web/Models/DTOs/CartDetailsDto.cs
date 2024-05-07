using DreamShop.Web.Models.DTOs;

namespace DreamShop.Web.Models
{
    public class CartDetailsDto
    {
        public long Id { get; set; }
        public long CartHeaderId { get; set; }
        public CartHeaderDto? CartHeader { get; set; }
        public long ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Count { get; set; }
    }
}