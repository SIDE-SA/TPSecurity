using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPAuth.Services.Common.Authorization;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Commands.Create;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Commands.Delete;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Queries.GetAllRefApplication;
using TPSecurity.Application.Core.SecuWeb.RefApplicationCore.Queries.GetByIdRefApplication;
using TPSecurity.Contracts.SecuWeb.RefApplication;

namespace TPSecurity.Api.Controllers.SecuWeb
{
    [Route("v1/reference-application")]
    [Authorize]
    public class RefApplicationController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public RefApplicationController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(RefApplicationResponse))]
        public async Task<IActionResult> Get([FromQuery] RefApplicationParameters refApplicationParameters)
        {
            var command = new GetAllRefApplicationQuery(refApplicationParameters);
            var result = await _mediator.Send(command);

            return result.Match(
                result => OkPagedList<IEnumerable<RefApplicationResponse>>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(RefApplicationResponse))]
        public async Task<IActionResult> Get(int id)
        {
            var command = new GetByIdRefApplicationQuery(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<RefApplicationResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(RefApplicationResponse))]
        public async Task<IActionResult> Create(CreateRefApplicationRequest request)
        {
            var command = new CreateRefApplicationCommand(request.Libelle, request.EstActif);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<RefApplicationResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(RefApplicationResponse))]
        public async Task<IActionResult> Update([FromHeader(Name = "If-Match")] string ifMatch, int id, UpdateRefApplicationRequest request)
        {
            var command = new UpdateRefApplicationCommand(id, request.Libelle, request.EstActif, ifMatch);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<RefApplicationResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteRefApplicationCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
