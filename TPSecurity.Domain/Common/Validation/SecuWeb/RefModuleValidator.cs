using FluentValidation;
using TPSecurity.Domain.Common.Entities.SecuWeb;

namespace TPSecurity.Domain.Common.Validation.SecuWeb
{
    public class RefModuleValidator : AbstractValidator<RefModule>
    {
        public RefModuleValidator()
        {
            RuleFor(x => x.Libelle).NotEmpty().MaximumLength(50);
        }
    }
}
