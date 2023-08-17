using Authentication.ApplicationService.DataTransferObjects.Requests.UserIdentityRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.Domain.Entities;
using Authentication.Domain.Enums;

namespace Authentication.ApplicationService.Mappers;
public sealed class UserIdentityMapper : IUserIdentityMapper
{
    public UserIdentity DtoUserIdentityRegisterRequestToDomain(UserIdentityRegisterRequest accountIdentityRegisterRequest) =>
         new()
         {
             UserName = accountIdentityRegisterRequest.Login,
             Email = null,
             PhoneNumber = null,
             EmailConfirmed = false,
             PhoneNumberConfirmed = false,
             PasswordHash = accountIdentityRegisterRequest.UserPassword.PasswordConfirm,
             UserType = EUserType.Client,
             UserStatus = EUserStatus.Active
         };

    public UserIdentityDataResponse DomainToDtoUserIdentityData(UserIdentity accountIdentity) =>
        new()
        {
            UserIdentityId = accountIdentity.Id,
            UserStatus = accountIdentity.UserStatus,
            UserType = accountIdentity.UserType
        };
}
