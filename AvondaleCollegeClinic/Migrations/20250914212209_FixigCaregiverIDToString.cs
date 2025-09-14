using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvondaleCollegeClinic.Migrations
{
    /// <inheritdoc />
    public partial class FixigCaregiverIDToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove foreign key constraint from Students → Caregivers
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Caregivers_CaregiverID",
                table: "Students");

            // Drop index on Students.CaregiverID
            migrationBuilder.DropIndex(
                name: "IX_Students_CaregiverID",
                table: "Students");

            // Drop CaregiverID column in Students
            migrationBuilder.DropColumn(
                name: "CaregiverID",
                table: "Students");

            // Finally drop the Caregivers table
            migrationBuilder.DropTable(
                name: "Caregivers");

        migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6e9ef90c-b4b4-402d-a998-fe7ba3dd19a1", "AQAAAAIAAYagAAAAEDPegFPVkUv6gAhQhX0f9oNqgt25xicHjB1Lcz8sxRt23h2rPc5imKsN8LlSff4IBQ==", "b69085cf-061b-4c22-853a-2081b6b2bb2c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Caregivers",
                columns: table => new
                {
                    CaregiverID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Relationship = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caregivers", x => x.CaregiverID);
                });

            // Re-add CaregiverID column in Students
            migrationBuilder.AddColumn<int>(
                name: "CaregiverID",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Recreate index
            migrationBuilder.CreateIndex(
                name: "IX_Students_CaregiverID",
                table: "Students",
                column: "CaregiverID");

            // Recreate foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_Students_Caregivers_CaregiverID",
                table: "Students",
                column: "CaregiverID",
                principalTable: "Caregivers",
                principalColumn: "CaregiverID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9e76af9b-af95-45be-a9c5-d5cea78fc65c", "AQAAAAIAAYagAAAAEF/bM8mAXANXCA6V592K9J0DhLHQTywMZbaBUY/jV7Ei47XYGEpas522a5ZkPvWLrA==", "10e361f7-9ea5-4b90-8604-e67ebc447ea5" });
        }
    }
}
