using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPAuth.Services.Common.Authorization;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Commands.Create;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Commands.Delete;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Queries.GetAllAccesModule;
using TPSecurity.Application.Core.SecuWeb.AccesModuleCore.Queries.GetByIdAccesModule;
using TPSecurity.Contracts.SecuWeb.AccesModule;

namespace TPSecurity.Api.Controllers.SecuWeb
{
    [Route("v1/acces-module")]
    [Authorize]
    public class AccesModuleController : ApiController
    {

        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AccesModuleController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(AccesModuleResponse))]
        public async Task<IActionResult> Get([FromQuery] AccesModuleParameters accesApplicationeParameters)
        {
            var command = new GetAllAccesModuleQuery(accesApplicationeParameters);
            var result = await _mediator.Send(command);

            return result.Match(
                result => OkPagedList<IEnumerable<AccesModuleResponse>>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(AccesModuleResponse))]
        public async Task<IActionResult> Get(int id)
        {
            var command = new GetByIdAccesModuleQuery(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesModuleResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(AccesModuleResponse))]
        public async Task<IActionResult> Create(CreateAccesModuleRequest request)
        {
            var command = new CreateAccesModuleCommand(request.EstActif, request.IdAccesGroupe, request.IdRefApplication);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesModuleResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(AccesModuleResponse))]
        public async Task<IActionResult> Update([FromHeader(Name = "If-Match")] string ifMatch, int id, UpdateAccesModuleRequest request)
        {
            var command = new UpdateAccesModuleCommand(id, request.EstActif, ifMatch);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesModuleResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteAccesModuleCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
