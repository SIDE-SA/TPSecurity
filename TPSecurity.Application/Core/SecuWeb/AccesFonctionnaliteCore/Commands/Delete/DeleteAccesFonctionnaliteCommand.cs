using ErrorOr;
using MediatR;

namespace TPSecurity.Application.Core.SecuWeb.AccesFonctionnaliteCore.Commands.Delete;

public record DeleteAccesFonctionnaliteCommand(int id) : IRequest<ErrorOr<Deleted>>;