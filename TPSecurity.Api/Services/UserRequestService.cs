using TPSecurity.Api.Http;
using TPSecurity.Application.Common.Interfaces.Services;

namespace TPSecurity.Api.Services;

public class UserRequestService : IUserRequestService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserRequestService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserName => _httpContextAccessor.HttpContext?.Request.Headers[HttpContextItemKeys.User];
}
