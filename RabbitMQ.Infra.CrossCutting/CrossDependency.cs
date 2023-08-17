#region

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMq.Domain.DTOs.Settings;
using RabbitMq.Domain.Interfaces.RabbitMq;
using RabbitMq.Domain.Interfaces.Services;
using RabbitMq.Service;
using RabbitMq.Service.RabbitMq;

#endregion



namespace RabbitMq.Infra.CrossCutting;

public static class CrossDependency
{
    public static IServiceCollection ConfigureDependencyInjection(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .ConfigureServices()
            .ConfigureOptions(configuration);

    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
        services.AddSingleton<IRabbitMqService, RabbitMqService>();
        
        return services;
    }

    private static IServiceCollection ConfigureOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));

        return services;
    }
}