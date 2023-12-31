﻿using System.Text.RegularExpressions;

namespace Authentication.Domain.Handlers.ValidationHandler;
public static partial class PasswordValidation
{
    public static bool ValidatePassword(this string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        var regex = PasswordRegex();
        var match = regex.Match(password);
        return match.Success;
    }

    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,20}$")]
    private static partial Regex PasswordRegex();
}
