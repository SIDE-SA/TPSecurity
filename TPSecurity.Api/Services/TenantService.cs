using TPSecurity.Api.Http;
using TPSecurity.Infrastructure.Interfaces;

namespace TPSecurity.API.Services;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid idSociete = Guid.Empty;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException("tenantService issue");
    }

    public Guid GetIdSociete()
    {                
        if(idSociete == Guid.Empty)
        {
            if (!Guid.TryParse(_httpContextAccessor.HttpContext?.Request.Headers[HttpContextItemKeys.IdSociete], out Guid result))
            {
                throw new ArgumentException("L'idSociete n'est pas un uuid valide");
            }

            idSociete = result;
        }

        return idSociete;
    }
}