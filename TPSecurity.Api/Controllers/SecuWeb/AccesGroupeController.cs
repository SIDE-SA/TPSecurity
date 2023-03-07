using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPAuth.Services.Common.Authorization;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Commands.Create;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Commands.Delete;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Queries.GetAllAccesGroupe;
using TPSecurity.Application.Core.SecuWeb.AccesGroupeCore.Queries.GetByIdAccesGroupe;
using TPSecurity.Contracts.SecuWeb.AccesGroupe;

namespace TPSecurity.Api.Controllers.SecuWeb
{
    [Route("v1/acces-groupe")]
    [Authorize]
    public class AccesGroupeController : ApiController
    {

        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AccesGroupeController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(AccesGroupeResponse))]
        public async Task<IActionResult> Get([FromQuery] AccesGroupeParameters accesGroupeParameters)
        {
            var command = new GetAllAccesGroupeQuery(accesGroupeParameters);
            var result = await _mediator.Send(command);

            return result.Match(
                result => OkPagedList<IEnumerable<AccesGroupeResponse>>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(AccesGroupeResponse))]
        public async Task<IActionResult> Get(int id)
        {
            var command = new GetByIdAccesGroupeQuery(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesGroupeResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(AccesGroupeResponse))]
        public async Task<IActionResult> Create(CreateAccesGroupeRequest request)
        {
            var command = new CreateAccesGroupeCommand(request.Libelle, request.EstActif, request.EstGroupeSpecial, request.IdSociete);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesGroupeResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(AccesGroupeResponse))]
        public async Task<IActionResult> Update([FromHeader(Name = "If-Match")] string ifMatch, int id, UpdateAccesGroupeRequest request)
        {
            var command = new UpdateAccesGroupeCommand(id, request.Libelle, request.EstActif, request.EstGroupeSpecial, ifMatch);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesGroupeResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteAccesGroupeCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
