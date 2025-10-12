using Microsoft.EntityFrameworkCore;
using Stakeholders.Api.Internal;
using Stakeholders.Api.Public;
using Stakeholders.Core.Domain.RepositoryInterfaces;
using Stakeholders.Core.UseCases;
using Stakeholders.Infrastructure;
using Stakeholders.Infrastructure.Repositories;
using Stakeholders.Mappers;

namespace Stakeholders.Startup
{
    public static class ModuleConfiguration
    {
        public static IServiceCollection RegisterModules(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(StakeholdersProfile));
            SetupCore(services);
            SetupInfrastructure(services);

            return services;
        }

        private static void SetupCore(IServiceCollection services)
        {
            services.AddScoped<Core.UseCases.FollowerClient>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IPersonSerivce, Core.UseCases.PersonService>();
            services.AddScoped<IUserService, Core.UseCases.UserService>();

        }

        private static void SetupInfrastructure(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserDatabaseRepository>();
            services.AddScoped<IPersonRepository, PersonDatabaseRepository>();

            services.AddDbContext<StakeholdersContext>(opt =>
                opt.UseNpgsql(DbConnectionStringBuilder.Build("stakeholders"),
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "stakeholders")));
        }
    }
}
