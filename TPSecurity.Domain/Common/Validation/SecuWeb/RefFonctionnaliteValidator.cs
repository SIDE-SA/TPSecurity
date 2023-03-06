using FluentValidation;
using TPSecurity.Domain.Common.Entities.SecuWeb;
using TPSecurity.Domain.Common.Enums;

namespace TPSecurity.Domain.Common.Validation.SecuWeb
{
    public class RefFonctionnaliteValidator : AbstractValidator<RefFonctionnalite>
    {
        public RefFonctionnaliteValidator()
        {
            RuleFor(x => x.Libelle).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Permission).NotEmpty().MaximumLength(20);
            RuleFor(x => EnumPermissions.IsAllowedValue(x.Permission)).Equal(true).WithMessage("La valeur de la permission n'est pas autorisée");
        }
    }
}
