using FluentValidation;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Domain.Common.Validation.SecuWeb
{
    public class RefFonctionnaliteValidator : AbstractValidator<RefFonctionnalite>
    {
        public RefFonctionnaliteValidator()
        {
            RuleFor(x => x.Libelle).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Permission).NotEmpty().MaximumLength(20);
        }
    }
}
