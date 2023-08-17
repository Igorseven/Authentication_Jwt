namespace Authentication.Domain.Providers;
public sealed class JwtTokenOptions
{
    public const string SectionName = "JwtToken";
    public required string JwtKey { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required double DurationInMinutes { get; init; }
}
