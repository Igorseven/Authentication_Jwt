using Authentication.ApplicationService.DataTransferObjects.Requests.UserRequest;
using Authentication.ApplicationService.DataTransferObjects.Responses.UserIdentityResponse;
using Authentication.ApplicationService.Interfaces.MapperContracts;
using Authentication.Domain.Entities;
using Authentication.Domain.Enums;

namespace Authentication.ApplicationService.Mappers;
public sealed class UserMapper : IUserIdentityMapper
{
    public User DtoRegisterToDomain(UserRegisterRequest accountRegisterRequest) =>
         new()
         {
             UserName = accountRegisterRequest.Login,
             Email = null,
             PhoneNumber = null,
             EmailConfirmed = false,
             PhoneNumberConfirmed = false,
             PasswordHash = accountRegisterRequest.UserPassword.PasswordConfirm,
             Type = EUserType.Client,
             Status = EUserStatus.Active
         };

    public UserSimpleResponse DomainToSimpleResponse(User account) =>
        new()
        {
            Id = account.Id,
            UserStatus = account.Status,
            UserType = account.Type
        };
}
