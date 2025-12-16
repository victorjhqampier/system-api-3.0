using Application.Internals.Adapters;
using FluentValidation;

namespace Application.Executors;

internal static class FluentValidExecutor
{
    public static ValidationResponseAdapter Execute<T>(T input, IValidator<T> validator, int statusGroup = 422)
    {
        var valResult = validator.Validate(input);

        if (valResult.IsValid) return new ValidationResponseAdapter
        {
            StatusGroup = statusGroup
        };

        return new ValidationResponseAdapter
        {
            StatusGroup = statusGroup,
            Validations = valResult.Errors
            .Select(x => new ValidationError
            {
                Code = x.ErrorCode,
                Message = x.ErrorMessage,
                Field = x.PropertyName
            })
            .ToList()
        };          
    }
}
