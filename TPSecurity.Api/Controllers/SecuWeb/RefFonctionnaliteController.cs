using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPAuth.Services.Common.Authorization;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Commands.Create;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Commands.Delete;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Commands.Update;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Queries.GetAllRefFonctionnalite;
using TPSecurity.Application.Core.SecuWeb.RefFonctionnaliteCore.Queries.GetByIdRefFonctionnalite;
using TPSecurity.Contracts.SecuWeb.RefFonctionnalite;

namespace TPSecurity.Api.Controllers.SecuWeb
{
    [Route("v1/reference-fonctionnalite")]
    [Authorize]
    public class RefFonctionnaliteController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public RefFonctionnaliteController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(RefFonctionnaliteResponse))]
        public async Task<IActionResult> Get([FromQuery] RefFonctionnaliteParameters refFonctionnaliteParameters)
        {
            var command = new GetAllRefFonctionnaliteQuery(refFonctionnaliteParameters);
            var result = await _mediator.Send(command);

            return result.Match(
                result => OkPagedList<IEnumerable<RefFonctionnaliteResponse>>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(RefFonctionnaliteResponse))]
        public async Task<IActionResult> Get(int id)
        {
            var command = new GetByIdRefFonctionnaliteQuery(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<RefFonctionnaliteResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(RefFonctionnaliteResponse))]
        public async Task<IActionResult> Create(CreateRefFonctionnaliteRequest request)
        {
            var command = new CreateRefFonctionnaliteCommand(request.Libelle, request.EstActif, request.EstDefaut, request.Permission, request.IdRefModule);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<RefFonctionnaliteResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(RefFonctionnaliteResponse))]
        public async Task<IActionResult> Update([FromHeader(Name = "If-Match")] string ifMatch, int id, UpdateRefFonctionnaliteRequest request)
        {
            var command = new UpdateRefFonctionnaliteCommand(id, request.Libelle, request.EstActif, request.EstDefaut, ifMatch);
            var result = await _mediator.Send(command);

            return result.Match(
                result => Ok<RefFonctionnaliteResponse>(result, _mapper),
                errors => Problem(errors)
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteRefFonctionnaliteCommand(id);
            var result = await _mediator.Send(command);

            return result.Match(
                result => NoContent(),
                errors => Problem(errors)
            );
        }
    }
}
