using Intaker.Application.Common;
using Intaker.Domain.Common;

namespace Intaker.Application.Tasks.Commands;

public class TaskCreateCommandInput(int taskStatusId, string name, string description, string? assignedTo)
{
    public int TaskStatusId = taskStatusId;
    public string Name = name;
    public string Description = description;
    public string? AssignedTo = assignedTo;
}

public class TaskCreateCommand(IUnitOfWork uow, IEventDispatcher eventDispatcher)
    : Command<TaskCreateCommandInput, int?>(uow, eventDispatcher)
{
    protected override async Task<int?> Implementation()
    {
        try
        {
            var task = new Domain.Tasks.Entities.Task
                (0, Input.Name, Input.Description, (Domain.Tasks.Enums.TaskStatus)Input.TaskStatusId, Input.AssignedTo);

            await UnitOfWork.TaskRepository.SaveAsync(EventDispatcher, task);

            return task.Id;
        }
        catch
        {
            return null;
        }
    }
}
