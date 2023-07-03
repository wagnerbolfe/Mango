using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCartAPI.Models.Dto;

namespace ShoppingCartAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
