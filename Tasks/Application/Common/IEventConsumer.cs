namespace Intaker.Application.Common;

public interface IEventConsumer
{
    public Task HandleMessageAsync(string content);
}
