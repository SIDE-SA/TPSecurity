using Azure.Core;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPAuth.Services.Common.Authorization;
using TPSecurity.Application.Core.SecuWeb.UtilisateurAccesCore.Queries.GetAllAccesByIdUtilisateur;
using TPSecurity.Contracts.SecuWeb.UtilisateurAcces;

namespace TPSecurity.Api.Controllers.SecuWeb
{
    [Route("v1/utilisateur/{idUtilisateur}/acces")]
    [Authorize]
    public class UtilisateurAccesController : ApiController
    {

        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public UtilisateurAccesController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(UtilisateurAccesResponse))]
        public async Task<IActionResult> Get(int idUtilisateur)
        {
            var command = new GetAllAccesByIdUtilisateurQuery(idUtilisateur);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok(result),
                errors => Problem(errors)
            );
        }
    }
}
