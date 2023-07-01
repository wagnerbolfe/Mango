using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        
        [Required(ErrorMessage = "The Coupon Code field is required!")]
        public string CouponCode { get; set; }

        [Required(ErrorMessage = "The Discount Amount field is required!")]
        public double DiscountAmount { get; set; }

        [Required(ErrorMessage = "The Minimum Amount field is required!")]
        public int MinAmount { get; set; }
    }
}
