using System.Threading.Tasks;
using Web.Models;
using Web.Utility;

namespace Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> CreateProductsAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StartDetail.ApiType.POST,
                Data = productDto,
                Url = StartDetail.ProductAPIBase + "/api/product",
                ContentType = StartDetail.ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDto> DeleteProductsAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StartDetail.ApiType.DELETE,
                Url = StartDetail.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StartDetail.ApiType.GET,
                Url = StartDetail.ProductAPIBase + "/api/product"
            });
        }

        public async Task<ResponseDto> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StartDetail.ApiType.GET,
                Url = StartDetail.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDto> UpdateProductsAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StartDetail.ApiType.PUT,
                Data = productDto,
                Url = StartDetail.ProductAPIBase + "/api/product",
                ContentType = StartDetail.ContentType.MultipartFormData
            });
        }
    }
}
