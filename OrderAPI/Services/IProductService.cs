using System.Collections.Generic;
using System.Threading.Tasks;
using OrderAPI.Models.Dto;

namespace OrderAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
