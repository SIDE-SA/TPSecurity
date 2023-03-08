using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPAuth.Services.Common.Authorization;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Commands.Create;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Commands.Delete;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Queries.GetAllAccesUtilisateur;
using TPSecurity.Application.Core.SecuWeb.AccesUtilisateurCore.Queries.GetByIdAccesUtilisateur;
using TPSecurity.Contracts.SecuWeb.AccesUtilisateur;

namespace TPSecurity.Api.Controllers.SecuWeb
{
    [Route("v1/acces-utilisateur")]
    [Authorize]
    public class AccesUtilisateurController : ApiController
    {

        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AccesUtilisateurController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(AccesUtilisateurResponse))]
        public async Task<IActionResult> Get([FromQuery] AccesUtilisateurParameters accesApplicationeParameters)
        {
            var command = new GetAllAccesUtilisateurQuery(accesApplicationeParameters);
            var result = await _mediator.Send(command);

            return result.Match(
                result => OkPagedList<IEnumerable<AccesUtilisateurResponse>>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(AccesUtilisateurResponse))]
        public async Task<IActionResult> Get(int id)
        {
            var command = new GetByIdAccesUtilisateurQuery(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesUtilisateurResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(AccesUtilisateurResponse))]
        public async Task<IActionResult> Create(CreateAccesUtilisateurRequest request)
        {
            var command = new CreateAccesUtilisateurCommand(request.EstActif, request.IdAccesGroupe, request.IdUtilisateur);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesUtilisateurResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(AccesUtilisateurResponse))]
        public async Task<IActionResult> Update([FromHeader(Name = "If-Match")] string ifMatch, int id, UpdateAccesUtilisateurRequest request)
        {
            var command = new UpdateAccesUtilisateurCommand(id, request.EstActif, ifMatch);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<AccesUtilisateurResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteAccesUtilisateurCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
