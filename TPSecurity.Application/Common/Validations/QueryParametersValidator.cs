using FluentValidation;
using TPSecurity.Application.Common.Parameters;

namespace TPSecurity.Application.Common.Validations;

public class QueryParametersValidator<T> : AbstractValidator<T> where T : QueryParameters
{
    public QueryParametersValidator()
    {
        RuleFor(x => x.offSet).GreaterThanOrEqualTo(0);
        RuleFor(x => x.limit).InclusiveBetween(0, 2000);
        RuleFor(x => x).Must(x => x.orderOrientation.ToLower() == "asc" || x.orderOrientation.ToLower() == "desc")
                       .WithMessage("OrderOrientation must be ASC or DESC");
    }
}
