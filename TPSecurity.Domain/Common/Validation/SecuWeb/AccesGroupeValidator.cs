using FluentValidation;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Domain.Common.Validation.SecuWeb
{
    public class AccesGroupeValidator : AbstractValidator<AccesGroupe>
    {
        public AccesGroupeValidator()
        {
            RuleFor(x => x.Libelle).NotEmpty().MaximumLength(50);
        }
    }
}
