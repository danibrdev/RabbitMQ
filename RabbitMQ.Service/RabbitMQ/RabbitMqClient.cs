#region

using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMq.Domain.DTOs.Settings;
using RabbitMq.Domain.Interfaces.RabbitMq;
using Constants = RabbitMq.Domain.Common.Constants;

#endregion

namespace RabbitMq.Service.RabbitMq;

public class RabbitMqClient : IRabbitMqClient, IDisposable
{
    private readonly ILogger<RabbitMqClient> _logger;
    private IConnection _connection = null!;
    private IModel _channel = null!;
    private readonly RabbitMqSettings _rabbitMqSettings;

    public RabbitMqClient(
        IOptions<RabbitMqSettings> mqSettings, 
        ILogger<RabbitMqClient> logger)
    {
        _logger = logger;
        _rabbitMqSettings = mqSettings.Value;
        
        Connect();
    }

    public void PublishMessage<T>(T model)
    {
        var message = JsonConvert.SerializeObject(model);
        var body = Encoding.UTF8.GetBytes(message);

        var basicProperties = _channel.CreateBasicProperties();
        basicProperties.DeliveryMode = 2;

        _channel.BasicPublish(
            exchange: $"{_rabbitMqSettings.Environment}.NomeExchange",
            routingKey: "routing.Exchange.Fila",
            basicProperties: basicProperties,
            body: body);
    }

    public void Dispose()
    {
        try
        {
            _channel.Close();
            _channel.Dispose();
            _channel = null!;
            
            _connection.Close();
            _connection.Dispose();
            _connection = null!; 
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, Constants.LoggerMessage, string.Format(ServiceResource.ErrorDisposeRabbit, ex.Message));
        }
    }
    
    #region Private Methods

    private void Connect()
    {
        if(_connection is not { IsOpen: true })
            try
            {
                _connection = new ConnectionFactory()
                {
                    VirtualHost = _rabbitMqSettings.Environment,
                    HostName = _rabbitMqSettings.HostName,
                    UserName = _rabbitMqSettings.UserName,
                    Password = _rabbitMqSettings.Password,
                    Port = _rabbitMqSettings.Port
                }.CreateConnection();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, Constants.LoggerMessage,
                    string.Format(ServiceResource.ErrorConectionRabbit, _rabbitMqSettings.HostName, _rabbitMqSettings.Port));

                return;
            }

        if (_channel is not { IsOpen: true })
            ConfigureQueue();

        return;
    }
    
    private void ConfigureQueue()
    {
        _channel = _connection.CreateModel();

        var args = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", $"{_rabbitMqSettings.Environment}.NomeDeadLetterExchange" }
        };
        
        _channel.ExchangeDeclare(
            exchange: $"{_rabbitMqSettings.Environment}.NomeExchange", 
            type: ExchangeType.Fanout,
            durable: true, 
            autoDelete:false);
        
        _channel.QueueDeclare(
            queue: $"{_rabbitMqSettings.Environment}.NomeFila", 
            durable: true, 
            exclusive: false, 
            autoDelete: false,
            arguments: args);
        
        _channel.QueueBind(
            queue: $"{_rabbitMqSettings.Environment}.NomeFila", 
            exchange: $"{_rabbitMqSettings.Environment}.NomeExchange", 
            routingKey: "routing.Exchange.Fila");
    }

    #endregion
}