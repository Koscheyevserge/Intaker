namespace Intaker.Infrastructure.Api.Tasks;

public class TaskCreateRequest
{
    public int TaskStatusId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? AssignedTo { get; set; }
}
