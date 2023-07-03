using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Diagnostics;
using System.Linq;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> list = new();

            var response = await _productService.GetAllProductsAsync();

            if (response is { IsSuccess: true })
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result) ?? string.Empty);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto model = new();

            var response = await _productService.GetProductByIdAsync(productId);

            if (response is { IsSuccess: true })
            {
                model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result) ?? string.Empty);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(model);
        }

        //[Authorize]
        //[HttpPost]
        //[ActionName("ProductDetails")]
        //public async Task<IActionResult> ProductDetails(ProductDto productDto)
        //{
        //    CartDto cartDto = new CartDto()
        //    {
        //        CartHeader = new CartHeaderDto
        //        {
        //            UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
        //        }
        //    };

        //    CartDetailsDto cartDetails = new CartDetailsDto()
        //    {
        //        Count = productDto.Count,
        //        ProductId = productDto.ProductId,
        //    };

        //    List<CartDetailsDto> cartDetailsDtos = new() { cartDetails };
        //    cartDto.CartDetails = cartDetailsDtos;

        //    ResponseDto? response = await _cartService.UpsertCartAsync(cartDto);

        //    if (response != null && response.IsSuccess)
        //    {
        //        TempData["success"] = "Item has been added to the Shopping Cart";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else
        //    {
        //        TempData["error"] = response?.Message;
        //    }

        //    return View(productDto);
        //}


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}