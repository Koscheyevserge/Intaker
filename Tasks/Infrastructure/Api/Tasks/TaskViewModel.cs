namespace Intaker.Infrastructure.Api.Tasks;

public class TaskViewModel(int id, string name, string description, int statusId, string? assignedTo)
{
    public TaskViewModel(Domain.Tasks.Entities.Task task)
        : this(task.Id, task.Name, task.Description, (int)task.Status, task.AssignedTo)
    {
    }

    public int Id { get; } = id;
    public string Name { get; } = name;
    public string Description { get; } = description;
    public int StatusId { get; } = statusId;
    public string StatusName { get; } = (Domain.Tasks.Enums.TaskStatus)statusId switch
    {
        Domain.Tasks.Enums.TaskStatus.Completed => "Completed",
        Domain.Tasks.Enums.TaskStatus.InProgress => "In Progress",
        Domain.Tasks.Enums.TaskStatus.NotStarted => "Not Started",
        _ => "Unknown",
    };
    public string? AssignedTo { get; } = assignedTo;
}
