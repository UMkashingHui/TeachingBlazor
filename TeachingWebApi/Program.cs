using AspNetCore.Identity.Mongo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using TeachingWebApi.Config;
using TeachingWebApi.Data.Seeder;
using TeachingWebApi.Models.Identity;
using TeachingWebApi.Services;
using TeachingWebApi.Utils.Data;
using TeachingWebApi.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.Configure<TeachingAppDatabaseSettings>(
    builder.Configuration.GetSection("TeachingAppDatabase"));

var MongoDbConnectionString = builder.Configuration.GetSection("TeachingAppDatabase").Get<TeachingAppDatabaseSettings>().ConnectionString;
var MongoDbUsersCollection = builder.Configuration.GetSection("TeachingAppDatabase").Get<TeachingAppDatabaseSettings>().UsersCollectionName;
var MongoDbRolesCollection = builder.Configuration.GetSection("TeachingAppDatabase").Get<TeachingAppDatabaseSettings>().RolesCollectionName;

builder.Services.AddSingleton<BooksService>();

// MongoDB with Identity
builder.Services.AddIdentityMongoDbProvider<TeachingBlazorUser, TeachingBlazorRole>(identity =>
            {
                // Password settings.
                identity.Password.RequireDigit = false;
                identity.Password.RequireLowercase = false;
                identity.Password.RequireNonAlphanumeric = false;
                identity.Password.RequireUppercase = false;
                identity.Password.RequiredLength = 1;
                identity.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                identity.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                identity.Lockout.MaxFailedAccessAttempts = 5;
                identity.Lockout.AllowedForNewUsers = true;

                // User settings.
                identity.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                identity.User.RequireUniqueEmail = false;
            },
                mongo =>
                {
                    mongo.ConnectionString = MongoDbConnectionString;
                    mongo.UsersCollection = MongoDbUsersCollection;
                    mongo.RolesCollection = MongoDbRolesCollection;
                }
            )
            .AddEntityFrameworkStores<MongoIdentityDbContext>()
            .AddDefaultTokenProviders();

builder.Services.AddTransient<MongoDbContext>();
builder.Services.AddTransient<MongoIdentityDbContext>();

builder.Services
    .AddTransient<DatabaseSeeder>()
        .AddDbContext<MongoIdentityDbContext>(options => options
            .UseMongoDB(new MongoClient(MongoDbConnectionString), "teaching_blazor_app"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Initialize(builder.Configuration);
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.UseRouting();

app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello From ASP.NET Core Web API");
                });
                endpoints.MapGet("/Resource1", async context =>
                {
                    await context.Response.WriteAsync("Hello From ASP.NET Core Web API Resource1");
                });
                endpoints.MapControllerRoute(
                  name: "Admin",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
            });

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
