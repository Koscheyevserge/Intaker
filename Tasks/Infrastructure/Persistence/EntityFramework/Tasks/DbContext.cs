using Microsoft.EntityFrameworkCore;

namespace Intaker.Infrastructure.Persistence.EntityFramework.Tasks;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbContext(DbContextOptions<DbContext> options) : base(options)
    {
        ModelBuilder modelBuilder = new();

        modelBuilder.Entity<Entities.TaskStatus>().ToTable("TaskStatuses");

        modelBuilder.Entity<Entities.Task>().ToTable("Tasks");
        modelBuilder.Entity<Entities.Task>().HasOne<Entities.TaskStatus>().WithMany().HasForeignKey(t => t.StatusId);
    }

    public DbSet<Entities.Task> Tasks { get; private set; }
    public DbSet<Entities.TaskStatus> TaskStatuses { get; private set; }
}
