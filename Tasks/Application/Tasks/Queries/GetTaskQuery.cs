using Intaker.Application.Common;

namespace Intaker.Application.Tasks.Queries;

public class GetTaskQueryInput
{
    public int TaskId { get; set; }
}

public class GetTaskQuery(IUnitOfWork uow) : Query<GetTaskQueryInput, Domain.Tasks.Entities.Task>(uow)
{
    protected override async Task<Domain.Tasks.Entities.Task> Implementation()
    {
        var task = await UnitOfWork.TaskRepository.FindByIdAsync(Input.TaskId);
        return task;
    }
}
