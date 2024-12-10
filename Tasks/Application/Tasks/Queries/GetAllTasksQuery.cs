using Intaker.Application.Common;

namespace Intaker.Application.Tasks.Queries;

public class GetAllTasksQueryInput
{
    
}

public class GetAllTasksQuery(IUnitOfWork uow) : Query<GetAllTasksQueryInput, IEnumerable<Domain.Tasks.Entities.Task>>(uow)
{
    protected override async Task<IEnumerable<Domain.Tasks.Entities.Task>> Implementation()
    {
        var tasks = await UnitOfWork.TaskRepository.GetAllAsync();
        return tasks;
    }
}
