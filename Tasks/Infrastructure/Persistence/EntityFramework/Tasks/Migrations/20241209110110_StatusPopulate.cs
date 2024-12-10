using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intaker.Infrastructure.Persistence.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class StatusPopulate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO TaskStatuses (Id, Name) VALUES (1, 'NotStarted'), (2, 'InProgress'), (3, 'Completed')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM TaskStatuses WHERE Id in (1, 2, 3)");
        }
    }
}
