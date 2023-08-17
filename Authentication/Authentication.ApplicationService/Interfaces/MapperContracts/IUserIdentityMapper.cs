using Authentication.ApplicationService.DataTransferObjects.Requests.UserIdentityRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
using Authentication.Domain.Entities;

namespace Authentication.ApplicationService.Interfaces.MapperContracts;
public interface IUserIdentityMapper
{
    UserIdentity DtoUserIdentityRegisterRequestToDomain(UserIdentityRegisterRequest userIdentityRegisterRequest);

    UserIdentityDataResponse DomainToDtoUserIdentityData(UserIdentity userIdentity);
}
