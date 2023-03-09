using FluentValidation;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Domain.Common.Validation.SecuWeb
{
    public class UtilisateurValidator : AbstractValidator<Utilisateur>
    {
        public UtilisateurValidator()
        {
            RuleFor(x => x.Nom).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Prenom).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(200);
        }
    }
}
