using Authentication.API.Settings;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Services.AddSettingsConfigurations(configuration);
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var cultureSetup = new[] { new CultureInfo("pt-BR") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
    SupportedCultures = cultureSetup,
    SupportedUICultures = cultureSetup
});


app.UseHttpsRedirection();
app.UseCors("DfPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//app.MigrateDatabase();
app.Run();
