using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface ICouponService
    {
        Task<ResponseDto> GetCouponAsync(string couponCode);
        Task<ResponseDto> GetAllCouponsAsync();
        Task<ResponseDto> GetCouponByIdAsync(int id);
        Task<ResponseDto> CreateCouponsAsync(CouponDto couponDto);
        Task<ResponseDto> UpdateCouponsAsync(CouponDto couponDto);
        Task<ResponseDto> DeleteCouponsAsync(int id);
    }   
}
