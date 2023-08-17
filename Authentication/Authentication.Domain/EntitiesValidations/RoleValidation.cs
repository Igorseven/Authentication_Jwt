using Authentication.Domain.Entities;
using Authentication.Domain.Enums;
using Authentication.Domain.Extensions;
using Authentication.Domain.Handlers.ValidationHandler;
using FluentValidation;

namespace Authentication.Domain.EntitiesValidations;
public sealed class RoleValidation : Validate<Role>
{
    public RoleValidation()
    {
        SetRules();
    }

    private void SetRules()
    {
        RuleFor(r => r.Name).Length(3, 256).NotEmpty()
            .WithMessage(r => string.IsNullOrWhiteSpace(r.Name)
            ? EMessage.Required.GetDescription().FormatTo("Nome da role")
            : EMessage.MoreExpected.GetDescription().FormatTo("", "entre {MinLength} e {MaxLength}"));
    }
}
