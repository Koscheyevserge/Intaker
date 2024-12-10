using Intaker.Domain.Common;
using Intaker.Domain.Tasks.Events;

namespace Intaker.Domain.Tasks.Entities;

/// <summary>
/// Domain task entity
/// </summary>
public class Task(
    int id,
    string name,
    string description,
    Enums.TaskStatus status,
    string? assignedTo)
{
    public int Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public string Description { get; private set; } = description;
    public Enums.TaskStatus Status { get; private set; } = status;
    public string? AssignedTo { get; private set; } = assignedTo;

    public void StatusUpdate(IEventDispatcher eventDispatcher, Enums.TaskStatus newStatus)
    {
        if (Status == newStatus)
            return;

        var @event = new TaskStatusChangedEvent(Id, Status, newStatus);

        Status = newStatus;

        eventDispatcher.RegisterEvent(@event);
    }
}
