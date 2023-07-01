using System.Threading.Tasks;
using Web.Models;
using Web.Utility;

namespace Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StartDetail.ApiType.POST,
                Data = registrationRequestDto,
                Url = StartDetail.AuthAPIBase + "/api/auth/AssignRole"
            });
        }

        public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StartDetail.ApiType.POST,
                Data = loginRequestDto,
                Url = StartDetail.AuthAPIBase + "/api/auth/login"
            }, withBearer: false);
        }

        public async Task<ResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StartDetail.ApiType.POST,
                Data = registrationRequestDto,
                Url = StartDetail.AuthAPIBase + "/api/auth/register"
            }, withBearer: false);
        }
    }
}
