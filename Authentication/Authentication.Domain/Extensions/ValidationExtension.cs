using FluentValidation.Results;

namespace Authentication.Domain.Extensions;
public static class ValidationExtension
{
    public static Dictionary<string, string> ToDictionary(this IEnumerable<ValidationFailure> errors)
    {
        var result = new Dictionary<string, string>();

        foreach (var error in errors)
        {
            result.TryAdd(error.PropertyName, error.ErrorMessage);
        }

        return result;
    }
}
