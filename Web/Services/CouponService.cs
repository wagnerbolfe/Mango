using System.Threading.Tasks;
using Web.Models;
using Web.Utility;

namespace Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StartDetail.ApiType.GET,
                Url = StartDetail.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StartDetail.ApiType.GET,
                Url = StartDetail.CouponAPIBase + "/api/coupon/GetByCode/" + couponCode
            });
        }

        public async Task<ResponseDto> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StartDetail.ApiType.GET,
                Url = StartDetail.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDto> CreateCouponsAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StartDetail.ApiType.POST,
                Data = couponDto,
                Url = StartDetail.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto> UpdateCouponsAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StartDetail.ApiType.PUT,
                Data = couponDto,
                Url = StartDetail.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto> DeleteCouponsAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StartDetail.ApiType.DELETE,
                Url = StartDetail.CouponAPIBase + "/api/coupon/" + id
            });
        }
    }
}
