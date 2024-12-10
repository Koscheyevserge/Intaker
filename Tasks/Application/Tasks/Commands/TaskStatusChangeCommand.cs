using Intaker.Application.Common;
using Intaker.Domain.Common;

namespace Intaker.Application.Tasks.Commands;

public class TaskStatusChangeCommandInput(int taskId, int taskStatusId)
{
    public int TaskId { get; set; } = taskId;
    public int TaskStatusId { get; set; } = taskStatusId;
}

public class TaskStatusChangeCommand(IUnitOfWork uow, IEventDispatcher eventDispatcher)
    : Command<TaskStatusChangeCommandInput, bool>(uow, eventDispatcher)
{
    protected override async Task<bool> Implementation()
    {
        try
        {
            var task = await UnitOfWork.TaskRepository.FindByIdAsync(Input.TaskId);

            task.StatusUpdate(EventDispatcher, (Domain.Tasks.Enums.TaskStatus)Input.TaskStatusId);

            await UnitOfWork.TaskRepository.SaveAsync(EventDispatcher, task);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
