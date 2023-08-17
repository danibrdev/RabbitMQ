#region

using RabbitMq.Domain.DTOs.InputModels;
using RabbitMq.Domain.Interfaces.RabbitMq;
using RabbitMq.Domain.Interfaces.Services;

#endregion

namespace RabbitMq.Service;

public class RabbitMqService : IRabbitMqService
{
    private readonly IRabbitMqClient _rabbitMqClient;

    public RabbitMqService(IRabbitMqClient rabbitMqClient)
        => _rabbitMqClient = rabbitMqClient;

    public Task SendMessageAsync(RabbitMqMessageInputModel mensagem)
    {
        _rabbitMqClient.PublishMessage(mensagem);

        return Task.CompletedTask; 
    }
}