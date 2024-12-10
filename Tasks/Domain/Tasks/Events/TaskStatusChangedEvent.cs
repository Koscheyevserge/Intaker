using Intaker.Domain.Common;

namespace Intaker.Domain.Tasks.Events;

public class TaskStatusChangedEvent
    (int taskId, Enums.TaskStatus oldStatus, Enums.TaskStatus newStatus) : Event
{
    public override string Name => nameof(TaskStatusChangedEvent);
    public readonly int taskId = taskId;
    public readonly Enums.TaskStatus oldStatus = oldStatus;
    public readonly Enums.TaskStatus newStatus = newStatus;
}