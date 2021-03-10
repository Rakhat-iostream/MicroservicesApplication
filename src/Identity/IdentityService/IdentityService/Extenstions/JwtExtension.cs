using IdentityService.Options;
using IdentityService.Repositories.Interfaces;
using IdentityService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;

namespace IdentityService.Extenstions
{
    public static class JwtExtension
    {
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new JwtOptions();
            var section = configuration.GetSection("jwt");
            section.Bind(options);
            services.Configure<JwtOptions>(section);
            services.AddSingleton<IJwtBuilder, JwtBuilder>();
            services.AddAuthentication()
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    IssuerSigningKey =
                       new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret))
                };
            });
        }
        public static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<UserDatabaseOptions>(configuration.GetSection("mongo"));
            services.AddSingleton(c =>
            {
                var options = c.GetService<IOptions<UserDatabaseOptions>>();
                return new MongoClient(options.Value.ConnectionString);
            });
            services.AddSingleton(c =>
            {
                var options = c.GetService<IOptions<UserDatabaseOptions>>();
                var client = c.GetService<MongoClient>();
                return client.GetDatabase(options.Value.Database);
            });
        }
    }
}
