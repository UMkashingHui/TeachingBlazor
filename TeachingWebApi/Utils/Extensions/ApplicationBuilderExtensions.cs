using TeachingWebApi.Data.Seeder;

namespace TeachingWebApi.Utils.Extensions
{
    internal static class ApplicationBuilderExtensions
    {

        internal static IApplicationBuilder Initialize(this IApplicationBuilder app, IConfiguration _configuration)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var initializers = serviceScope.ServiceProvider.GetServices<DatabaseSeeder>();

            foreach (var initializer in initializers)
            {
                initializer.Initialize();
            }


            return app;
        }

    }
}