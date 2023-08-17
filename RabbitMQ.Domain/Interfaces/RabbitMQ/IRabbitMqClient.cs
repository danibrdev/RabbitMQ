namespace RabbitMq.Domain.Interfaces.RabbitMq;

public interface IRabbitMqClient
{
    void PublishMessage<T>(T model);
}