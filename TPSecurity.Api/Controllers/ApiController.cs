using ErrorOr;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using TPSecurity.Api.Http;

namespace TPSecurity.Api.Controllers
{
    [ApiController]
    [ProducesResponseType(500, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(401, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
    public class ApiController : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            if (errors.Count is 0)
            {
                return Problem();
            }

            if (errors.All(error => error.Type == ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            HttpContext.Items[HttpContextItemKeys.Errors] = errors;
            var firstError = errors[0];
            return Problem(firstError);
        }

        private IActionResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            };

            return Problem(statusCode: statusCode, title: error.Description);
        }

        private IActionResult ValidationProblem(List<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();
            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(
                    error.Code,
                    error.Description);
            }
            return ValidationProblem(modelStateDictionary);
        }

        //Methode qui ajoute 
        protected IActionResult Ok<T>(dynamic result, IMapper _mapper)
        {
            var hashCode = result.HashCode ?? "";
            HttpContext.Response.Headers.Add(HttpContextItemKeys.hashCode, hashCode);
            return Ok(_mapper.Map<T>(result));
        }

        protected IActionResult OkPagedList<T>(dynamic result, IMapper _mapper)
        {
            var paginationData = new
            {
                result.TotalCount
            };

            HttpContext.Response.Headers.Add(HttpContextItemKeys.Pagination, JsonConvert.SerializeObject(paginationData));
            return Ok(_mapper.Map<T>(result));
        }
    }
}
