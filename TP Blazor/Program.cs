using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TP_Blazor.Data;
using Blazorise;
using Blazorise.Icons.FontAwesome;
using Microsoft.Extensions.DependencyInjection;
using Blazorise.Bootstrap;
using Blazored.LocalStorage;
using TP_Blazor.Services;
using Blazored.Modal;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
//builder.Services.AddScoped<IDataService, DataLocalService>();
builder.Services.AddScoped<IDataService, DataApiService>();
builder.Services.AddHttpClient();
builder.Services.AddBlazorise()
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredModal();

builder.Services.AddControllers();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    // Set the default culture of the web site
    options.DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US"));
    
    // Declare the supported culture
    options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("fr-FR") };
    options.SupportedUICultures = new List<CultureInfo> { new CultureInfo("en-US"), new CultureInfo("fr-FR") };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();


app.UseRouting();

var options = ((IApplicationBuilder)app).ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

if (options?.Value != null)
{
    // use the default localization
    app.UseRequestLocalization(options.Value);
}

// Add the controller to the endpoint
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
