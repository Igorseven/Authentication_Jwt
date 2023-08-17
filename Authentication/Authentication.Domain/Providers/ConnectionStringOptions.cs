namespace Authentication.Domain.Providers;
public sealed class ConnectionStringOptions
{
    public const string SectionName = "ConnectionStrings";
    public required string DefaultConnection { get; init; } = string.Empty;
}
