using Board.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Board.Api.Setups;

internal static class PipelineSetup
{
    extension(WebApplication app)
    {
        public async Task ConfigureAsync()
        {
            if (!app.Environment.IsDevelopment())
                app.UseHttpsRedirection();

            app.ConfigureHealthCheck();
            app.ConfigureApiDocumentation();
            app.UseExceptionHandler();
            app.MapControllers();
            await app.ApplyMigrationsAsync();
        }

        private void ConfigureApiDocumentation()
        {
            if (!app.Environment.IsDevelopment())
                return;

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        private void ConfigureHealthCheck()
        {
            app.MapHealthChecks("/", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
    }
}