using Board.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Board.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public void ConfigureApplicationLayer()
        {
            services.AddMediator(options =>
            {
                options.ServiceLifetime = ServiceLifetime.Scoped;
                options.PipelineBehaviors = [typeof(ValidationBehavior<,>)];
            });

            services.AddValidatorsFromAssembly(
                typeof(DependencyInjection).Assembly,
                includeInternalTypes: true
            );
        }
    }
}