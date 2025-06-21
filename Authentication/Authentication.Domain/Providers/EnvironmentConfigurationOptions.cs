namespace Authentication.Domain.Providers;

public sealed class EnvironmentConfigurationOptions
{
    public const string SectionName = "EnvironmentConfiguration";
    public required bool Active { get; init; }
}