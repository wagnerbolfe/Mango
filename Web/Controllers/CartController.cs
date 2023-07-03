using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        //[HttpPost]
        //[ActionName("Checkout")]
        //public async Task<IActionResult> Checkout(CartDto cartDto)
        //{

        //    var cart = await LoadCartDtoBasedOnLoggedInUser();
        //    cart.CartHeader.Phone = cartDto.CartHeader.Phone;
        //    cart.CartHeader.Email = cartDto.CartHeader.Email;
        //    cart.CartHeader.Name = cartDto.CartHeader.Name;

        //    var response = await _orderService.CreateOrder(cart);
        //    OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

        //    if (response != null && response.IsSuccess)
        //    {
        //        //get stripe session and redirect to stripe to place order
        //        //
        //        var domain = Request.Scheme + "://" + Request.Host.Value + "/";

        //        StripeRequestDto stripeRequestDto = new()
        //        {
        //            ApprovedUrl = domain + "cart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
        //            CancelUrl = domain + "cart/checkout",
        //            OrderHeader = orderHeaderDto
        //        };

        //        var stripeResponse = await _orderService.CreateStripeSession(stripeRequestDto);
        //        StripeRequestDto stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDto>
        //                                    (Convert.ToString(stripeResponse.Result));
        //        Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
        //        return new StatusCodeResult(303);

        //    }
        //    return View();
        //}

        //public async Task<IActionResult> Confirmation(int orderId)
        //{
        //    ResponseDto? response = await _orderService.ValidateStripeSession(orderId);
        //    if (response != null & response.IsSuccess)
        //    {

        //        OrderHeaderDto orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
        //        if (orderHeader.Status == SD.Status_Approved)
        //        {
        //            return View(orderId);
        //        }
        //    }
        //    //redirect to some error page based on status
        //    return View(orderId);
        //}

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var response = await _cartService.RemoveFromCartAsync(cartDetailsId);
            if (response is { IsSuccess: true })
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var response = await _cartService.ApplyCouponAsync(cartDto);
            if (response is { IsSuccess: true })
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            var response = await _cartService.ApplyCouponAsync(cartDto);
            if (response is { IsSuccess: true })
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            var cart = await LoadCartDtoBasedOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;

            var response = await _cartService.EmailCart(cart);
            if (response is { IsSuccess: true })
            {
                TempData["success"] = "Email will be processed and sent shortly.";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var response = await _cartService.GetCartByUserIdAsync(userId);

            if (response is { IsSuccess: true })
            {
                var cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result) ?? string.Empty);
                return cartDto;
            }

            return new CartDto();
        }
    }
}
