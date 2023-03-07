using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPAuth.Services.Common.Authorization;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Commands.Create;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Commands.Delete;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Queries.GetAllAccesApplication;
using TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Queries.GetByIdAccesApplication;
using TPSecurity.Contracts.SecuWeb.AccesApplication;

namespace TPSecurity.Api.Controllers.SecuWeb
{
    [Route("v1/acces-application")]
    [Authorize]
    public class AccesApplicationController : ApiController
    {

        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AccesApplicationController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(AccesApplicationResponse))]
        public async Task<IActionResult> Get([FromQuery] AccesApplicationParameters accesApplicationeParameters)
        {
            var command = new GetAllAccesApplicationQuery(accesApplicationeParameters);
            var result = await _mediator.Send(command);

            return result.Match(
                result => OkPagedList<IEnumerable<AccesApplicationResponse>>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(AccesApplicationResponse))]
        public async Task<IActionResult> Get(int id)
        {
            var command = new GetByIdAccesApplicationQuery(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesApplicationResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(AccesApplicationResponse))]
        public async Task<IActionResult> Create(CreateAccesApplicationRequest request)
        {
            var command = new CreateAccesApplicationCommand(request.EstActif, request.IdAccesGroupe, request.IdRefApplication);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesApplicationResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(AccesApplicationResponse))]
        public async Task<IActionResult> Update([FromHeader(Name = "If-Match")] string ifMatch, int id, UpdateAccesApplicationRequest request)
        {
            var command = new UpdateAccesApplicationCommand(id, request.EstActif, ifMatch);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesApplicationResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteAccesApplicationCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
