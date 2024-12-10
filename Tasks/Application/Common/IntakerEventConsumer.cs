using System.Text.Json;
using Intaker.Application.Tasks.EventHandlers;
using Intaker.Domain.Common;
using Intaker.Domain.Tasks.Events;

namespace Intaker.Application.Common;

public class IntakerEventConsumer : IEventConsumer
{
    public Task HandleMessageAsync(string content)
    {
        var @event = JsonSerializer.Deserialize<Event>(content)
            ?? throw new Exception("Can't parse event");

        switch (@event.Name)
        {
            case nameof(TaskStatusChangedEvent):
                var handler = new TaskStatusChangeEventHandler();
                return handler.HandleMessageAsync(content);
        }
        return Task.CompletedTask;
    }
}
