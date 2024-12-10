using Intaker.Domain.Common;

namespace Intaker.Application.Common;

public abstract class Command<TInput, TOutput>(IUnitOfWork uow, IEventDispatcher eventDispatcher)
{
    protected readonly IUnitOfWork UnitOfWork = uow;
    protected readonly IEventDispatcher EventDispatcher = eventDispatcher;

    protected TInput Input { get; set; } = default!;

    public async Task<TOutput> ExecuteAsync(TInput input)
    {
        Input = input;
        UnitOfWork.BeginTransaction();
        TOutput result;

        try
        {
            result = await Implementation();
            UnitOfWork.CommitTransaction();
            EventDispatcher.DispatchEvents();
        }
        catch
        {
            UnitOfWork.RollbackTransaction();
            throw;
        }

        return result;
    }

    protected abstract Task<TOutput> Implementation();
}
