using ErrorOr;
using FluentValidation.Results;

namespace TPSecurity.Domain.Common.Validation
{
    public static class ValidationHelper
    {
        public static List<Error> GenerateErrorList(ValidationResult result)
        {
            List<Error> errors = new();
            foreach (var error in result.Errors.Select(x => x.ErrorMessage))
            {
                errors.Add(Error.Validation(error));
            }

            return errors;
        }
    }
}
