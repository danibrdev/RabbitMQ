#region

using RabbitMq.Domain.DTOs.InputModels;

#endregion

namespace RabbitMq.Domain.Interfaces.Services;

public interface IRabbitMqService
{
    Task SendMessageAsync(RabbitMqMessageInputModel mensagem); 
}