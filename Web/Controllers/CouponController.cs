using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto> list = new();

            var response = await _couponService.GetAllCouponsAsync();

            if (response is { IsSuccess: true })
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result) ?? string.Empty);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }

        public Task<IActionResult> CouponCreate()
        {
            return Task.FromResult<IActionResult>(View());
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _couponService.CreateCouponsAsync(model);

                if (response is { IsSuccess: true })
                {
                    TempData["success"] = "Coupon created successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            var response = await _couponService.GetCouponByIdAsync(couponId);

            if (response is { IsSuccess: true })
            {
                var model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result) ?? string.Empty);
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            var response = await _couponService.DeleteCouponsAsync(couponDto.CouponId);

            if (response is { IsSuccess: true })
            {
                TempData["success"] = "Coupon deleted successfully";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(couponDto);
        }

    }
}
