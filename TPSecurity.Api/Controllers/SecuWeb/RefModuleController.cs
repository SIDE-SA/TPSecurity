using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPAuth.Services.Common.Authorization;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Commands.Create;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Commands.Delete;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Queries.GetAllRefModule;
using TPSecurity.Application.Core.SecuWeb.RefModuleCore.Queries.GetByIdRefModule;
using TPSecurity.Contracts.SecuWeb.RefModule;

namespace TPSecurity.Api.Controllers.SecuWeb
{
    [Route("v1/reference-module")]
    [Authorize]
    public class RefModuleController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public RefModuleController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(RefModuleResponse))]
        public async Task<IActionResult> Get([FromQuery] RefModuleParameters refModuleParameters)
        {
            var command = new GetAllRefModuleQuery(refModuleParameters);
            var result = await _mediator.Send(command);

            return result.Match(
                result => OkPagedList<IEnumerable<RefModuleResponse>>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(RefModuleResponse))]
        public async Task<IActionResult> Get(int id)
        {
            var command = new GetByIdRefModuleQuery(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<RefModuleResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(RefModuleResponse))]
        public async Task<IActionResult> Create(CreateRefModuleRequest request)
        {
            var command = new CreateRefModuleCommand(request.Libelle, request.EstActif, request.IdRefApplication);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<RefModuleResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(RefModuleResponse))]
        public async Task<IActionResult> Update([FromHeader(Name = "If-Match")] string ifMatch, int id, UpdateRefModuleRequest request)
        {
            var command = new UpdateRefModuleCommand(id, request.Libelle, request.EstActif, ifMatch);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<RefModuleResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteRefModuleCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
