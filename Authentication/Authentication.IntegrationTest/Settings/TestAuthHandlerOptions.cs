using Microsoft.AspNetCore.Authentication;

namespace Authentication.IntegrationTest.Settings;

public class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
    public readonly string _defaultUserId = "25693911-6336-4d2b-af29-023c5babe3f3";
}