using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Stakeholders.Startup
{
    public static class AuthConfiguration
    {
        public static IServiceCollection ConfigureAuth(this IServiceCollection services)
        {
            ConfigureAuthentication(services);
            ConfigureAuthorizationPolicies(services);
            return services;
        }

        private static void ConfigureAuthentication(IServiceCollection services)
        {
            var key = "ultra_extra_long_super_secret_soa_key";
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "soa";
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "soa-front.com";

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("AuthenticationTokens-Expired", "true");
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
        }

        private static void ConfigureAuthorizationPolicies(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("administratorPolicy", policy => policy.RequireRole("administrator"));
                options.AddPolicy("authorPolicy", policy => policy.RequireRole("author"));
                options.AddPolicy("touristPolicy", policy => policy.RequireRole("tourist"));
                options.AddPolicy("authorOrTouristPolicy", policy => policy.RequireRole("author", "tourist"));
            });
        }
    }
}