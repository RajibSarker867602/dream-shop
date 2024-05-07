namespace DreamShop.Web.Models
{
    public class CartHeaderDto
    {
        public CartHeaderDto()
        {
            CartDetails = new List<CartDetailsDto>();
        }

        public long Id { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double CartTotal { get; set; }
        public ICollection<CartDetailsDto> CartDetails { get; set; }
    }
}