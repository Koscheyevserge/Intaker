using Intaker.Domain.Common;

namespace Intaker.Domain.Tasks.Events;

public class TaskCreatedEvent(int taskId) : Event
{
    public override string Name => nameof(TaskCreatedEvent);
    public readonly int taskId = taskId;
}