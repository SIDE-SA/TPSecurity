using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPAuth.Services.Common.Authorization;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Commands.Create;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Commands.Delete;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Queries.GetAllAccesFonctionnalite;
using TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Queries.GetByIdAccesFonctionnalite;
using TPSecurity.Contracts.SecuWeb.AccesFonctionnalite;

namespace TPSecurity.Api.Controllers.SecuWeb
{
    [Route("v1/acces-fonctionnalite")]
    [Authorize]
    public class AccesFonctionnaliteController : ApiController
    {

        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AccesFonctionnaliteController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(AccesFonctionnaliteResponse))]
        public async Task<IActionResult> Get([FromQuery] AccesFonctionnaliteParameters accesApplicationeParameters)
        {
            var command = new GetAllAccesFonctionnaliteQuery(accesApplicationeParameters);
            var result = await _mediator.Send(command);

            return result.Match(
                result => OkPagedList<IEnumerable<AccesFonctionnaliteResponse>>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(AccesFonctionnaliteResponse))]
        public async Task<IActionResult> Get(int id)
        {
            var command = new GetByIdAccesFonctionnaliteQuery(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesFonctionnaliteResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(AccesFonctionnaliteResponse))]
        public async Task<IActionResult> Create(CreateAccesFonctionnaliteRequest request)
        {
            var command = new CreateAccesFonctionnaliteCommand(request.EstActif, request.IdAccesModule, request.IdRefFonctionnalite);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesFonctionnaliteResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(AccesFonctionnaliteResponse))]
        public async Task<IActionResult> Update([FromHeader(Name = "If-Match")] string ifMatch, int id, UpdateAccesFonctionnaliteRequest request)
        {
            var command = new UpdateAccesFonctionnaliteCommand(id, request.EstActif, ifMatch);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesFonctionnaliteResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteAccesFonctionnaliteCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
