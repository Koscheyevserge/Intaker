using Intaker.Application.Common;
using Intaker.Application.Tasks.Boundaries;
using Intaker.Application.Tasks.Commands;
using Intaker.Application.Tasks.Queries;
using Intaker.Domain.Common;
using Microsoft.EntityFrameworkCore;
using TaskRepository = Intaker.Infrastructure.Persistence.EntityFramework.Tasks.Repository;

namespace Intaker.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Persistence.EntityFramework.Tasks.DbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("DbContext"), sql => sql.CommandTimeout(300)));

        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUnitOfWork, IntakerUnitOfWork>();

        //Provide RabbitMQProducer here after configuring RabbitMQ in the appsettings.Development.json
        services.AddSingleton<IEventProducer, FakeProducer>();

        services.AddSingleton<IEventDispatcher, IntakerEventDispatcher>();
        services.AddSingleton<IEventConsumer, IntakerEventConsumer>();
        services.AddHostedService<RabbitMQBridgeHostedService>();

        services.AddScoped<TaskCreateCommand>();
        services.AddScoped<TaskStatusChangeCommand>();

        services.AddScoped<GetAllTasksQuery>();
        services.AddScoped<GetTaskQuery>();

        return services;
    }
}
