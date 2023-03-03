using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TPSecurity.Api.Controllers
{
    public class ErrorsController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error")]     
        public IActionResult Error()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            if(exception is not null && exception.InnerException?.GetType() == typeof(ArgumentException))
            {
                return Problem(exception.InnerException.Message);
            }

            return Problem(exception?.Message);
        }
    }
}
