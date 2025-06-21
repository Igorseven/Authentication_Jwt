using Authentication.ApplicationService.DataTransferObjects.Requests.UserRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
using Authentication.Domain.Entities;

namespace Authentication.ApplicationService.Interfaces.MapperContracts;
public interface IUserIdentityMapper
{
    User DtoRegisterToDomain(UserRegisterRequest userRegisterRequest);

    UserSimpleResponse DomainToSimpleResponse(User user);
}
