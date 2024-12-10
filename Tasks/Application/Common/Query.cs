namespace Intaker.Application.Common;

public abstract class Query<TInput, TOutput>(IUnitOfWork uow)
{
    protected readonly IUnitOfWork UnitOfWork = uow;
    protected TInput Input { get; set; } = default!;

    public async Task<TOutput> ExecuteAsync(TInput input)
    {
        Input = input;
        var output = await Implementation();
        return output;
    }

    protected abstract Task<TOutput> Implementation();
}
