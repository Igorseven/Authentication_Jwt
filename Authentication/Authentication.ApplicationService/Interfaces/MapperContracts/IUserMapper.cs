using Authentication.ApplicationService.DataTransferObjects.Requests.UserRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
using Authentication.Domain.Entities;

namespace Authentication.ApplicationService.Interfaces.MapperContracts;
public interface IUserMapper
{
    User DtoRegisterToDomain(UserRegisterRequest userRegisterRequest);

    UserSimpleResponse DomainToSimpleResponse(User user);
}
