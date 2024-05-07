using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DreamShop.Services.ShoppingCartAPI.Models.Dto;

namespace DreamShop.Services.ShoppingCartAPI.Models
{
    public class CartHeader
    {
        public CartHeader()
        {
            CartDetails = new List<CartDetails>();
        }
        [Key] 
        public long Id { get; set; }

        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public ICollection<CartDetails> CartDetails { get; set; }

        [NotMapped] public double Discount { get; set; }
        [NotMapped] public double CartTotal { get; set; }
    }
}
