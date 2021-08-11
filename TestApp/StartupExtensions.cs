using Microsoft.Extensions.DependencyInjection;
using TestApp.DBContext;
using TestApp.IServices;

namespace TestApp
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddTestServices(this IServiceCollection services)
        {
            services.AddScoped<ITestServices, TestServices>();
            return services;
        }
    }
}
