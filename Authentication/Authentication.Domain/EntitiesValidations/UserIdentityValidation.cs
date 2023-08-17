using Authentication.Domain.Entities;
using Authentication.Domain.Enums;
using Authentication.Domain.Extensions;
using Authentication.Domain.Handlers.ValidationHandler;
using FluentValidation;

namespace Authentication.Domain.EntitiesValidations;
public sealed class UserIdentityValidation : Validate<UserIdentity>
{
    public UserIdentityValidation()
    {
        SetRules();
    }

    private void SetRules()
    {
        RuleFor(a => a.UserStatus).IsInEnum().WithMessage(EMessage.InvalidFormat.GetDescription().FormatTo("Status do usuário"));

        RuleFor(c => c.UserType).IsInEnum().WithMessage(EMessage.Required.GetDescription().FormatTo("Tipo de usuário"));

        RuleFor(a => a.UserName).EmailAddress().Length(7, 150)
            .WithMessage(a => !string.IsNullOrWhiteSpace(a.UserName)
            ? EMessage.MoreExpected.GetDescription().FormatTo("Login", "entre {MinLength} e {MaxLength}")
            : EMessage.Required.GetDescription().FormatTo("Login"));

        RuleFor(a => a.PasswordHash).Length(8, 20).Must(password => password.ValidatePassword())
            .WithMessage("A senha não atende aos requisitos.");

    }
}
