using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
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

        public Task<IActionResult> ProductCreate()
        {
            return Task.FromResult<IActionResult>(View());
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProductsAsync(model);

                if (response is { IsSuccess: true })
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            var response = await _productService.GetProductByIdAsync(productId);

            if (response is { IsSuccess: true })
            {
                var model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result) ?? string.Empty);
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            var response = await _productService.DeleteProductsAsync(productDto.ProductId);

            if (response is { IsSuccess: true })
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDto);
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            var response = await _productService.GetProductByIdAsync(productId);

            if (response is { IsSuccess: true })
            {
                var model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result) ?? string.Empty);
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateProductsAsync(productDto);

                if (response is { IsSuccess: true })
                {
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(productDto);
        }

    }
}
