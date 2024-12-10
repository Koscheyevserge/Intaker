namespace Intaker.Domain.Common;

public class IntakerEventDispatcher(IEventProducer eventProducer) : IEventDispatcher
{
    private IEventProducer EventProducer = eventProducer;
    private List<Event> Events = [];

    public void DispatchEvents()
    {
        foreach (var @event in Events)
        {
            EventProducer.Send(@event);
        }
        Events.Clear();
    }

    public void RegisterEvent(Event @event)
    {
        Events.Add(@event);
    }
}
