using MediatR;
using System.Text.Json;

namespace TPSecurity.Api.Behaviour;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {       
        //Request        
        _logger.LogInformation($" Handling {typeof(TRequest).FullName}");
        LogObject(request);

        dynamic response = await next();

        LogObject(response);            

        return response;
    }

    private void LogObject(dynamic objectToLog)
    {
        string json = JsonSerializer.Serialize(objectToLog);
        _logger.LogInformation(json);
    }
}
