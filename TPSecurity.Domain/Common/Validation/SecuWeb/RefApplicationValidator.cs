using FluentValidation;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Domain.Common.Validation.SecuWeb
{
    public class RefApplicationValidator : AbstractValidator<RefApplication>
    {
        public RefApplicationValidator()
        {
            RuleFor(x => x.Libelle).NotEmpty().MaximumLength(50);
        }
    }
}
