using Intaker.Application.Tasks.Boundaries;
using Intaker.Domain.Common;
using Microsoft.EntityFrameworkCore.Storage;
using DomainTask = Intaker.Domain.Tasks.Entities.Task;
using EntityTask = Intaker.Infrastructure.Persistence.EntityFramework.Tasks.Entities.Task;
using Task = System.Threading.Tasks.Task;

namespace Intaker.Infrastructure.Persistence.EntityFramework.Tasks;

public class Repository(DbContext context) : ITaskRepository
{
    #region Transaction
    private IDbContextTransaction? _transaction;
    public void BeginTransaction()
    {
        _transaction = Context.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        _transaction?.Commit();
    }

    public void RollbackTransaction()
    {
        _transaction?.Rollback();
    }
    #endregion
    #region Core
    public readonly DbContext Context = context;
    private int identity = context.Tasks.Any() ? context.Tasks.Max(t => t.Id) : 0;

    private static DomainTask MapToDomain(EntityTask task)
    {
        return new DomainTask
            (task.Id, task.Name, task.Description, (Domain.Tasks.Enums.TaskStatus)task.StatusId, task.AssignedTo);
    }

    private static EntityTask MapToEntity(DomainTask task)
    {
        return new EntityTask(task.Id, task.Name, task.Description, (int)task.Status, task.AssignedTo);
    }
    #endregion
    #region ITaskRepository
    public Task<bool> ExistsByIdAsync(int id)
    {
        var exists = Context.Tasks.Any(t => t.Id == id);
        return Task.FromResult(exists);
    }

    public Task<DomainTask> FindByIdAsync(int id)
    {
        var task = Context.Tasks.Single(t => t.Id == id);
        var taskDomain = MapToDomain(task);
        return Task.FromResult(taskDomain);
    }

    public Task<IEnumerable<DomainTask>> GetAllAsync()
    {
        var tasks = Context.Tasks.ToList().Select(MapToDomain);
        return Task.FromResult(tasks);
    }

    public async Task SaveAsync(IEventDispatcher eventDispatcher, DomainTask task)
    {
        if (task.Id > 0)
        {
            var entity = Context.Tasks.Single(t => t.Id == task.Id);
            entity.Name = task.Name;
            entity.Description = task.Description;
            entity.StatusId = (int)task.Status;
            entity.AssignedTo = task.AssignedTo;
            Context.Tasks.Update(entity);
        }
        else
        {
            var entity = MapToEntity(task);
            //Generate a new id for both the entity and the domain object before saving
            var id = ++identity;
            var taskProperty = task.GetType().GetProperty("Id")
                ?? throw new InvalidOperationException("Id property not found in the domain object");
            taskProperty.SetValue(task, id);

            var entityProperty = entity.GetType().GetProperty("Id")
                ?? throw new InvalidOperationException("Id property not found in the entity object");
            entityProperty.SetValue(entity, id);
            eventDispatcher.RegisterEvent(new Domain.Tasks.Events.TaskCreatedEvent(id));

            Context.Tasks.Add(entity);
        }

        await Context.SaveChangesAsync();
    }
    #endregion
}
