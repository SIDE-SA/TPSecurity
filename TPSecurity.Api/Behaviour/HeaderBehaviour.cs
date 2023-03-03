using MediatR;
using TPSecurity.Api.Http;
using ErrorOr;

namespace TPSecurity.Api.Behaviour;

public class HeaderBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public HeaderBehaviour(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {        
        if (!_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(HttpContextItemKeys.User, out var value))
        {            
            return (dynamic)Error.Validation(code: HttpContextItemKeys.User, description: $"{HttpContextItemKeys.User} manquant en tant que header");
        }

        return await next();   
        

    }
}
