namespace Intaker.Infrastructure.Persistence.EntityFramework.Tasks.Entities;

using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Task entity
/// </summary>
public class Task(int id, string name, string description, int statusId, string? assignedTo)
{
    #region Default Values
    /// <summary>
    /// The default name for a task if not provided
    /// </summary>
    private const string DEFAULT_NAME = "DEFAULT_NAME";
    /// <summary>
    /// The default description for a task if not provided
    /// </summary>
    private const string DEFAULT_DESCRIPTION = "DEFAULT_DESCRIPTION";
    /// <summary>
    /// The default assigned to for a task if not provided
    /// </summary>
    private const string? DEFAULT_ASSIGNED_TO = null;
    /// <summary>
    /// The default status for a task if not provided
    /// </summary>
    private const int DEFAULT_STATUS_ID = (int)Domain.Tasks.Enums.TaskStatus.NotStarted;
    /// <summary>
    /// The default id for a task if not provided
    /// </summary>
    private const int DEFAULT_ID = 0;
    #endregion

    /// <summary>
    /// EF Core requires an empty constructor
    /// </summary>
    private Task() : this(DEFAULT_ID, DEFAULT_NAME, DEFAULT_DESCRIPTION, DEFAULT_STATUS_ID, DEFAULT_ASSIGNED_TO)
    {

    }

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; } = id;

    public string Name { get; set; } = name;
    public string Description { get; set; } = description;

    public int StatusId { get; set; } = statusId;
    public TaskStatus? Status { get; set; } = null;

    public string? AssignedTo { get; set; } = assignedTo;
}
