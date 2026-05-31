using System.Text;
using Board.Api.Middlewares;
using Board.Application;
using Board.Infrastructure;
using Board.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace Board.Api.Setups;

internal static class BuilderSetup
{
    extension(WebApplicationBuilder builder)
    {
        public void Configure()
        {
            builder.ConfigureApiDocumentation();
            builder.ConfigureExceptionHandling();
            builder.ConfigureOptions();
            builder.ConfigureAuthentication();
            builder.ConfigureLayers();
        }

        private void ConfigureApiDocumentation()
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Informe o token JWT no campo abaixo."
                });

                options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
                {
                    { new OpenApiSecuritySchemeReference("Bearer", doc), [] }
                });
            });
        }

        private void ConfigureExceptionHandling()
        {
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        }

        private void ConfigureOptions()
        {
            builder.Services
                .AddOptions<TokenSettingsOptions>()
                .Bind(builder.Configuration.GetSection("TokenSettings"))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        private void ConfigureAuthentication()
        {
            var tokenSettings = builder.Configuration
                .GetSection("TokenSettings")
                .Get<TokenSettingsOptions>()!;

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.SecretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = tokenSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = tokenSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true
                });

            builder.Services.AddAuthorization();
        }

        private void ConfigureLayers()
        {
            builder.Services.ConfigureApplicationLayer();
            builder.ConfigureInfrastructureLayer();
        }
    }
}