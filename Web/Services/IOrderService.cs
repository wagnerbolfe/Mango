﻿using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface IOrderService
    {
        Task<ResponseDto> CreateOrder(CartDto cartDto);
        Task<ResponseDto> CreateStripeSession(StripeRequestDto stripeRequestDto);
        Task<ResponseDto> ValidateStripeSession(int orderHeaderId);
        Task<ResponseDto> GetAllOrder(string userId);
        Task<ResponseDto> GetOrder(int orderId);
        Task<ResponseDto> UpdateOrderStatus(int orderId, string newStatus);
    }
}
