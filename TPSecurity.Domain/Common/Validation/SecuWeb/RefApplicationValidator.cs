using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
