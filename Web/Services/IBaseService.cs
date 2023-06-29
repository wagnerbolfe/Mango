using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface IBaseService
    {
        Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}
