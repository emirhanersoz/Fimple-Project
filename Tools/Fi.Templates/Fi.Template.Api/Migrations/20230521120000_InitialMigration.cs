using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fi.TemplateUniqueName.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "__EFServiceMigrationHistory",
                columns: table => new
                {
                    MigrationId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ProductVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MigrateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CompensationLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateCount = table.Column<int>(type: "int", nullable: false),
                    BusinessKey = table.Column<long>(type: "bigint", nullable: false),
                    MessageKey = table.Column<long>(type: "bigint", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Entity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompensationState = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompensationLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityChangeLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BusinessKey = table.Column<long>(type: "bigint", nullable: false),
                    MessageKey = table.Column<long>(type: "bigint", nullable: false),
                    ProcessCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    AccountNumber = table.Column<long>(type: "bigint", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Entity = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityChangeLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sample",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateCount = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SampleType = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sample", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompensationLog_BusinessKey_MessageKey_CompensationState",
                table: "CompensationLog",
                columns: new[] { "BusinessKey", "MessageKey", "CompensationState" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityChangeLog_BusinessKey_MessageKey_TableName",
                table: "EntityChangeLog",
                columns: new[] { "BusinessKey", "MessageKey", "TableName" });

            migrationBuilder.CreateIndex(
                name: "IX_Sample_Code",
                table: "Sample",
                column: "Code",
                unique: true);

            migrationBuilder.Sql(System.IO.File.ReadAllText(System.IO.Path.Combine(AppContext.BaseDirectory, "Migrations/MsSql/Seed/InitialSeed.sql")));

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__EFServiceMigrationHistory");

            migrationBuilder.DropTable(
                name: "CompensationLog");

            migrationBuilder.DropTable(
                name: "EntityChangeLog");

            migrationBuilder.DropTable(
                name: "Sample");
        }
    }
}
