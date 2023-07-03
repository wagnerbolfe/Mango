using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface IProductService
    {
        Task<ResponseDto> GetAllProductsAsync();
        Task<ResponseDto> GetProductByIdAsync(int id);
        Task<ResponseDto> CreateProductsAsync(ProductDto productDto);
        Task<ResponseDto> UpdateProductsAsync(ProductDto productDto);
        Task<ResponseDto> DeleteProductsAsync(int id);
    }
}
