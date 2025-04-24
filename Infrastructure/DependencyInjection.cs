using Application.Interfaces;
using Application.Services;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<TemplateService>();
            services.AddScoped<IDocumentGenerator, DocumentGenerator>();
            services.AddScoped<ITemplateRepository, TemplateRepository>();
            return services;
        }
    }
}
