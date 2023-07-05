using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Models;
using Web.Services;
using Web.Utility;

namespace Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        public IActionResult OrderIndex()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeaderDto> list;
            var userId = "";

            if (!User.IsInRole(StaticDetails.RoleAdmin))
            {
                userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            }

            var response = _orderService.GetAllOrder(userId).GetAwaiter().GetResult();
            if (response is { IsSuccess: true })
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.Result) ?? string.Empty);
                switch (status)
                {
                    case "approved":
                        list = list?.Where(u => u.Status == StaticDetails.Status_Approved);
                        break;
                    case "readyforpickup":
                        list = list?.Where(u => u.Status == StaticDetails.Status_ReadyForPickup);
                        break;
                    case "cancelled":
                        list = list?.Where(u => u.Status is StaticDetails.Status_Cancelled or StaticDetails.Status_Refunded);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                list = new List<OrderHeaderDto>();
            }
            return Json(new { data = list!.OrderByDescending(u => u.OrderHeaderId) });
        }

        [Authorize]
        public async Task<IActionResult> OrderDetail(int orderId)
        {
            var orderHeaderDto = new OrderHeaderDto();
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            var response = await _orderService.GetOrder(orderId);
            if (response is { IsSuccess: true })
            {
                orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result) ?? string.Empty);
            }
            if (!User.IsInRole(StaticDetails.RoleAdmin) && userId != orderHeaderDto!.UserId)
            {
                return NotFound();
            }
            return View(orderHeaderDto);
        }

        [HttpPost("OrderReadyForPickup")]
        public async Task<IActionResult> OrderReadyForPickup(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, StaticDetails.Status_ReadyForPickup);
            if (response is { IsSuccess: true })
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId });
            }
            return View();
        }

        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, StaticDetails.Status_Completed);
            if (response is { IsSuccess: true })
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId });
            }
            return View();
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var response = await _orderService.UpdateOrderStatus(orderId, StaticDetails.Status_Cancelled);
            if (response is { IsSuccess: true })
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId });
            }
            return View();
        }

    }
}
