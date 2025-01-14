using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfileService.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureForeignKeys : Migration
    {
        /// <inheritdoc />
       protected override void Up(MigrationBuilder migrationBuilder)
{
    // Execute PRAGMA operation directly
    migrationBuilder.Sql("PRAGMA foreign_keys = 0;", suppressTransaction: true);

    // Add any other necessary commands if needed
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    // Reset PRAGMA operation if needed
    migrationBuilder.Sql("PRAGMA foreign_keys = 1;", suppressTransaction: true);
}
    }
}
