using System.Text.RegularExpressions;

namespace Authentication.Domain.Handlers.ValidationHandler;
public static class PasswordValidation
{
    public static bool ValidatePassword(this string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        var regex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,20}$");
        var match = regex.Match(password);
        return match.Success;
    }
}
