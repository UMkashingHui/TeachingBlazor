using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using TeachingBlazorApp.Infrastructure.Managers;
using TeachingBlazorApp.Infrastructure.Managers.Identity.Authentication;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddLocalization(options =>
                {
                    options.ResourcesPath = "Resources";
                });

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(builder.Configuration["WebApi"] ?? "http://localhost:5211")
    });

// Register Services
builder.Services.AddScoped<HealthCheckManager>();
builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();

// Add third party libraries
builder.Services.AddMudServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
