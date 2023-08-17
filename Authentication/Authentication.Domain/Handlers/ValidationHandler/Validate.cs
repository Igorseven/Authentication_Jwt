using Authentication.Domain.Extensions;
using Authentication.Domain.Interfaces.OthersContracts;
using FluentValidation;
using FluentValidation.Results;

namespace Authentication.Domain.Handlers.ValidationHandler;
public abstract class Validate<T> : AbstractValidator<T>, IValidate<T> where T : class
{
    private ValidationResult? _validationResult;

    private void CreateResult(T entity) => _validationResult ??= base.Validate(entity);

    private async Task CreateResultAsync(T entity) => _validationResult ??= await base.ValidateAsync(entity);

    private Dictionary<string, string> GetErrors() => _validationResult!.Errors.ToDictionary();


    public async Task<ValidationResponse> ValidationAsync(T entity)
    {
        await CreateResultAsync(entity);

        return ValidationResponse.CreateResponse(GetErrors());
    }

    public ValidationResponse Validation(T entity)
    {
        CreateResult(entity);

        return ValidationResponse.CreateResponse(GetErrors());
    }
}

