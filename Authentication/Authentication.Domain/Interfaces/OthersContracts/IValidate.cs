using Authentication.Domain.Handlers.ValidationHandler;

namespace Authentication.Domain.Interfaces.OthersContracts;
public interface IValidate<T> where T : class
{
    Task<ValidationResponse> ValidationAsync(T entity);
    ValidationResponse Validation(T entity);
}
