using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPAuth.Services.Common.Authorization;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Commands.Create;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Commands.Delete;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Queries.GetAllUtilisateur;
using TPSecurity.Application.Core.SecuWeb.UtilisateurCore.Queries.GetByIdUtilisateur;
using TPSecurity.Contracts.SecuWeb.Utilisateur;

namespace TPSecurity.Api.Controllers.SecuWeb
{
    [Route("v1/utilisateur")]
    [Authorize]
    public class UtilisateurController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public UtilisateurController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(UtilisateurResponse))]
        public async Task<IActionResult> Get([FromQuery] UtilisateurParameters utilisateurParameters)
        {
            var command = new GetAllUtilisateurQuery(utilisateurParameters);
            var result = await _mediator.Send(command);

            return result.Match(
                result => OkPagedList<IEnumerable<UtilisateurResponse>>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(UtilisateurResponse))]
        public async Task<IActionResult> Get(int id)
        {
            var command = new GetByIdUtilisateurQuery(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<UtilisateurResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(UtilisateurResponse))]
        public async Task<IActionResult> Create(CreateUtilisateurRequest request)
        {
            var command = new CreateUtilisateurCommand(request.Nom, request.Prenom, request.Email, request.EstActif);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<UtilisateurResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(UtilisateurResponse))]
        public async Task<IActionResult> Update([FromHeader(Name = "If-Match")] string ifMatch, int id, UpdateUtilisateurRequest request)
        {
            var command = new UpdateUtilisateurCommand(id, request.Nom, request.Prenom, request.Email, request.EstActif, ifMatch);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<UtilisateurResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteUtilisateurCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
