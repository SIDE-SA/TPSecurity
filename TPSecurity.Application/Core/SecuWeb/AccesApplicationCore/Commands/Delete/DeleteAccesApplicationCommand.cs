using ErrorOr;
using MediatR;

namespace TPSecurity.Application.Core.SecuWeb.AccesApplicationCore.Commands.Delete;

public record DeleteAccesApplicationCommand(int id) : IRequest<ErrorOr<Deleted>>;