namespace Intaker.Infrastructure.Api.Tasks;

public class TaskStatusChangeRequest
{
    public int TaskId { get; set; }
    public int TaskStatusId { get; set; }
}
