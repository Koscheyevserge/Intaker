using Intaker.Application.Tasks.Boundaries;

namespace Intaker.Application.Common;

public class IntakerUnitOfWork(ITaskRepository taskRepository) : IUnitOfWork
{
    public ITaskRepository TaskRepository => taskRepository;

    public void CommitTransaction()
    {
        TaskRepository.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        TaskRepository.RollbackTransaction();
    }

    public void BeginTransaction()
    {
        TaskRepository.BeginTransaction();
    }
}
