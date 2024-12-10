using System.ComponentModel.DataAnnotations.Schema;

namespace Intaker.Infrastructure.Persistence.EntityFramework.Tasks.Entities;

/// <summary>
/// Status for tasks
/// </summary>
public class TaskStatus
{
    /// <summary>
    /// The default name for a task status if not provided
    /// </summary>
    private const string DEFAULT_NAME = "DEFAULT_NAME";

    /// <summary>
    /// EF Core requires an empty constructor
    /// </summary>
    private TaskStatus() : this(0, DEFAULT_NAME)
    {

    }

    public TaskStatus(int id, string name)
    {
        Id = id;
        if (string.IsNullOrWhiteSpace(name))
            name = DEFAULT_NAME;
        Name = name;
        TaskEntities = new List<Task>();
    }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; private set; }
    public string Name { get; private set; }
    public ICollection<Task> TaskEntities { get; private set; }
}
