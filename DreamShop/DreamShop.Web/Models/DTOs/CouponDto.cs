namespace DreamShop.Web.Models.DTOs
{
    public class CouponDto
    {
        public long Id { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
