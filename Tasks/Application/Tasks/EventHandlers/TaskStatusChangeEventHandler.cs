using Intaker.Application.Common;
using Intaker.Domain.Common;
using Intaker.Domain.Tasks.Events;
using Newtonsoft.Json;

namespace Intaker.Application.Tasks.EventHandlers;

public class TaskStatusChangeEventHandler : IEventConsumer
{
    public Task HandleMessageAsync(string content)
    {
        var @event = JsonConvert.DeserializeObject<TaskStatusChangedEvent>(content);
        return Task.CompletedTask;
    }
}
