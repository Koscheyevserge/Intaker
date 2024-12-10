using Intaker.Domain.Common;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Intaker.Infrastructure.Services;

public record RabbitMQProducerOptions(IConfiguration configuration)
{
    public readonly bool Enabled = configuration.GetValue<bool>("RabbitMq:Enabled");
    public readonly string HostName = configuration.GetValue<string>("RabbitMq:HostName")!;
    public readonly string UserName = configuration.GetValue<string>("RabbitMq:UserName")!;
    public readonly string Password = configuration.GetValue<string>("RabbitMq:Password")!;
    public readonly string QueueName = configuration.GetValue<string>("RabbitMq:QueueName")!;
    public readonly string Exchange = configuration.GetValue<string>("RabbitMq:Exchange")!;
    public readonly string RoutingKey = configuration.GetValue<string>("RabbitMq:RoutingKey")!;
}

public class RabbitMQProducer : IDisposable, IEventProducer
{
    private IModel? _channel;
    private IConnection? _connection;
    private readonly ILogger _logger;
    private readonly RabbitMQProducerOptions _options;
    private readonly ConnectionFactory? _connectionFactory;

    public RabbitMQProducer(ILogger<RabbitMQProducer> logger, IConfiguration configuration)
    {
        _options = new RabbitMQProducerOptions(configuration);
        _logger = logger;
        if (!_options.Enabled)
        {
            _logger.LogWarning($"Service RabbitMq is disabled in configuration file");
            return;
        }
        _connectionFactory = new ConnectionFactory() { HostName = _options.HostName, UserName = _options.UserName, Password = _options.Password };
        bool connectionOk = false;
        int tryAmount = 0;
        string errorMessage = string.Empty;
        while (!connectionOk && tryAmount < 10)
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
                connectionOk = true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                tryAmount++;
                Thread.Sleep(100 * tryAmount);
            }
        }
        if (!connectionOk)
        {
            _logger.LogError($"Cannot open connection to RabbitMq. Error: {errorMessage}");
            return;
        }
        bool channelOk = false;
        tryAmount = 0;
        while (!channelOk && tryAmount < 10 && _connection != null)
        {
            try
            {
                _channel = _connection.CreateModel();
                channelOk = true;
            }
            catch
            {
                tryAmount++;
                Thread.Sleep(100 * tryAmount);
            }
        }
        if (!channelOk)
        {
            _logger.LogError($"Cannot open channel to RabbitMq. Error: {errorMessage}");
            return;
        }
    }
    public virtual void ConfigureQueue()
    {
        if (!_options.Enabled || _channel == null)
        {
            return;
        }

        var queueName = $"{_options.QueueName}.{_options.RoutingKey}";
        try
        {
            _channel.ExchangeDeclare(exchange: _options.Exchange,
                                               type: ExchangeType.Direct,
                                               durable: true,
                                               autoDelete: false);

            _channel.QueueDeclare(queue: queueName,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

            _channel.QueueBind(queue: queueName,
                                      exchange: _options.Exchange,
                                      routingKey: _options.RoutingKey);
        }
        catch (Exception e)
        {
            _logger.LogError($"RabbitMq cant connect to {_options.Exchange}, queue: {queueName}, routingKey {_options.RoutingKey}. Message: {e.Message}");
        }
    }

    public virtual void Send(object message)
    {
        if (!_options.Enabled || _connection == null || _channel == null || _connectionFactory == null)
        {
            return;
        }
        if (!_connection.IsOpen)
        {
            _connection = _connectionFactory.CreateConnection();
        }
        if (!_channel.IsOpen)
        {
            _channel = _connection.CreateModel();
        }
        ConfigureQueue();

        string msgJson = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(msgJson);

        _channel.BasicPublish(exchange: _options.Exchange,
                             routingKey: _options.RoutingKey,
                             basicProperties: null,
                             body: body);
        Dispose();
    }

    public void Dispose()
    {
        if (!_options.Enabled)
        {
            return;
        }
        if (_channel != null && _channel.IsOpen)
        {
            _channel.Close();
            _channel.Dispose();
        }
        if (_connection != null && _connection.IsOpen)
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}