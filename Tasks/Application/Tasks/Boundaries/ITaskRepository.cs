using Intaker.Domain.Common;

namespace Intaker.Application.Tasks.Boundaries;

public interface ITaskRepository
{
    Task<bool> ExistsByIdAsync(int id);

    Task<Domain.Tasks.Entities.Task> FindByIdAsync(int id);

    Task<IEnumerable<Domain.Tasks.Entities.Task>> GetAllAsync();

    Task SaveAsync(IEventDispatcher eventDispatcher, Domain.Tasks.Entities.Task task);

    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();
}
