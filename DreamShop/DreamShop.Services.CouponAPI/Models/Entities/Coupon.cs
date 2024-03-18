using System.ComponentModel.DataAnnotations;

namespace DreamShop.Services.CouponAPI.Models.Entities
{
    public class Coupon
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
