using Authentication.API.IoC;
using Authentication.API.Settings;
using Authentication.API.Settings.Configurations;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Services.AddInversionOfControlHandler();
builder.Services.AddSettingsControl(configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.AddWebApplication(configuration);
app.MigrateDatabase();
app.Run();

public partial class Program
{
    
}
