using AutoMapper;
using DocumentProcessor.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DocumentProcessor.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlLiteConnString");
            services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString), optionsLifetime: ServiceLifetime.Scoped, contextLifetime: ServiceLifetime.Scoped);
        }

        public static void ConfigureScopes(this IServiceCollection services)
        {
            services.AddScoped<Repository>();
        }

        public static void AddMapper(this IServiceCollection services)
        {
            var mapper = new MapperConfiguration(cfg => new MapperConfigurator(cfg).GetConfiguration());
            services.AddSingleton(mapper.CreateMapper());
        }
    }
}
