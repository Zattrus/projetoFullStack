using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Suburb = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ContactFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactLastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leads", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Leads",
                columns: new[] { "Id", "Category", "ContactEmail", "ContactFirstName", "ContactLastName", "ContactPhone", "CreatedAt", "Description", "Price", "Status", "Suburb" },
                values: new object[] { new Guid("467f6177-98e7-4412-ae14-d5406aa1295b"), "Plumbing", "john.doe@example.com", "John", "Doe", null, new DateTime(2025, 10, 25, 3, 4, 48, 146, DateTimeKind.Utc).AddTicks(8259), "Fix leaking faucet", 150.00m, 0, "Sydney" });

            migrationBuilder.InsertData(
                table: "Leads",
                columns: new[] { "Id", "Category", "ContactEmail", "ContactFirstName", "ContactLastName", "ContactPhone", "CreatedAt", "Description", "Price", "Status", "Suburb" },
                values: new object[] { new Guid("a27b1e4a-1b1f-4fe6-9f1b-67d5948cb4d8"), "Electrical", "jane.smith@example.com", "Jane", "Smith", "123-456-7890", new DateTime(2025, 10, 25, 3, 4, 48, 146, DateTimeKind.Utc).AddTicks(8259), "Install new light fixtures", 300.00m, 1, "Melbourne" });

            migrationBuilder.CreateIndex(
                name: "IX_Leads_Status",
                table: "Leads",
                column: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leads");
        }
    }
}
