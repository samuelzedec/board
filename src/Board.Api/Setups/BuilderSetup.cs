using Board.Api.Middlewares;
using Board.Application;
using Board.Infrastructure;

namespace Board.Api.Setups;

internal static class BuilderSetup
{
    extension(WebApplicationBuilder builder)
    {
        public void Configure()
        {
            builder.ConfigureApiDocumentation();
            builder.ConfigureExceptionHandling();
            builder.ConfigureLayers();
        }

        private void ConfigureApiDocumentation()
        {
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        private void ConfigureExceptionHandling()
        {
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        }

        private void ConfigureLayers()
        {
            builder.Services.ConfigureApplicationLayer();
            builder.ConfigureInfrastructureLayer();
        }
    }
}