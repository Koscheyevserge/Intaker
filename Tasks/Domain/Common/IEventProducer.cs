namespace Intaker.Domain.Common;

public interface IEventProducer
{
    public void Send(object message);
}
