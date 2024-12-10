using Intaker.Application.Tasks.Boundaries;

namespace Intaker.Application.Common;

public interface IUnitOfWork
{
    ITaskRepository TaskRepository { get; }

    void BeginTransaction();
    void CommitTransaction();
    void RollbackTransaction();
}
