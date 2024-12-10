namespace Intaker.Domain.Common;

public interface IEventDispatcher
{
    void RegisterEvent(Event @event);

    void DispatchEvents();
}
