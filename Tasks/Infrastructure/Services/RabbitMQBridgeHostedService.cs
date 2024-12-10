using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Intaker.Application.Common;

namespace Intaker.Infrastructure.Services;

public record RabbitMQConsumerOptions(IConfiguration configuration)
{
    public readonly bool Enabled = configuration.GetValue<bool>("RabbitMq:Enabled");
    public readonly bool FullLogging = configuration.GetValue<bool>("RabbitMq:FullLogging");
    public readonly string HostName = configuration.GetValue<string>("RabbitMq:HostName")!;
    public readonly string UserName = configuration.GetValue<string>("RabbitMq:UserName")!;
    public readonly string Password = configuration.GetValue<string>("RabbitMq:Password")!;
    public readonly string QueueName = configuration.GetValue<string>("RabbitMq:QueueName")!;
    public readonly string Exchange = configuration.GetValue<string>("RabbitMq:Exchange")!;
    public readonly string RoutingKey = configuration.GetValue<string>("RabbitMq:RoutingKey")!;
}

public class RabbitMQBridgeHostedService : BackgroundService
{
    protected readonly IConnection? _connection;
    protected readonly IModel? _channel;
    protected readonly string? _queueName;
    protected readonly ILogger _logger;
    protected readonly RabbitMQConsumerOptions _options;
    private readonly IEventConsumer _implementation;

    public RabbitMQBridgeHostedService(ILoggerFactory loggerFactory, IConfiguration configuration, IEventConsumer implementation)
    {
        _options = new RabbitMQConsumerOptions(configuration);
        _implementation = implementation;
        _logger = loggerFactory.CreateLogger<RabbitMQBridgeHostedService>();
        if (!_options.Enabled)
        {
            _logger.LogWarning($"Service RabbitMq is disabled in configuration file");
            return;
        }
        if (_options.FullLogging)
        {
            _logger.LogWarning(JsonConvert.SerializeObject(_options));
        }
        _queueName = $"{_options.QueueName}.{_options.RoutingKey}";
        try
        {
            _connection = new ConnectionFactory { HostName = _options.HostName, UserName = _options.UserName, Password = _options.Password }.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_options.Exchange, ExchangeType.Direct, true, false, null);
            _channel.QueueDeclare(_queueName, false, false, false, null);
            _channel.QueueBind(_queueName, _options.Exchange, _options.RoutingKey, null);
            _channel.BasicQos(0, 1, false);
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"RabbitMq consumer: {ex.Message}, {ex.StackTrace}");
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.Enabled)
        {
            return Task.CompletedTask;
        }

        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (ch, ea) =>
        {
            try
            {
                bool ok = false;
                int tries = 0;
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                while (!ok && tries < 10)
                {
                    try
                    {
                        await _implementation.HandleMessageAsync(content);
                        _channel?.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception e)
                    {
                        _logger.LogWarning(e.Message);
                        _logger.LogWarning($"CAN'T handle message {content}");
                        tries++;
                        Thread.Sleep(100 * tries);
                    }                    
                    ok = true;
                }
                if (!ok)
                {
                    _logger.LogCritical($"CAN'T handle message {content}");
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical($"{e.Message}");
            }
        };

#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
        consumer.Shutdown += OnConsumerShutdown;
        consumer.Registered += OnConsumerRegistered;
        consumer.Unregistered += OnConsumerUnregistered;
        consumer.ConsumerCancelled += OnConsumerConsumerCancelled;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

        _channel.BasicConsume(_queueName, false, consumer);

        if (_options.FullLogging)
        {
            _logger.LogWarning($"Started consuming {_queueName}");
        }
        return Task.CompletedTask;
    }

    protected void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
    {
        _logger.LogWarning($"Consumer {_queueName} On Consumer Cancelled");
    }
    protected void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
    {
        _logger.LogWarning($"Consumer {_queueName} On Unregistered");
    }
    protected void OnConsumerRegistered(object sender, ConsumerEventArgs e)
    {
        _logger.LogWarning($"Consumer {_queueName} On Registered");
    }
    protected void OnConsumerShutdown(object sender, ShutdownEventArgs e)
    {
        _logger.LogWarning($"Consumer {_queueName} On Shutdown");
    }
    protected void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        _logger.LogWarning("Connection On Shutdown");
    }

    public override void Dispose()
    {
        if (!_options.Enabled)
        {
            return;
        }
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}