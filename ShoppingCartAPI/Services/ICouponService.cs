using System.Threading.Tasks;
using ShoppingCartAPI.Models.Dto;

namespace ShoppingCartAPI.Services
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
