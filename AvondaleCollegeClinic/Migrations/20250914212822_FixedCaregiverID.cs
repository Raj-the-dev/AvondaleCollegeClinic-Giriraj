using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvondaleCollegeClinic.Migrations
{
    /// <inheritdoc />
    public partial class FixedCaregiverID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Recreate Caregivers table with CaregiverID as string
            migrationBuilder.CreateTable(
                name: "Caregivers",
                columns: table => new
                {
                    CaregiverID = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Relationship = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caregivers", x => x.CaregiverID);
                    table.ForeignKey(
                        name: "FK_Caregivers_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            // Add CaregiverID column back to Students (string FK)
            migrationBuilder.AddColumn<string>(
                name: "CaregiverID",
                table: "Students",
                type: "nvarchar(9)",
                nullable: false,
                defaultValue: "");

            // Create index and FK on Students → Caregivers
            migrationBuilder.CreateIndex(
                name: "IX_Students_CaregiverID",
                table: "Students",
                column: "CaregiverID");

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
                values: new object[] { "f288b714-2311-4b04-940d-4f45bb2abc3e", "AQAAAAIAAYagAAAAEM9asEIHXk4OcNzyD3ToZlkgzjB4h8ygTHZ470PU1IEcfiaDI/Zm/qilOvMrkpbSCw==", "7e33a2f8-2d86-498e-8490-0fc4129dc874" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop FK and column in Students
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Caregivers_CaregiverID",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_CaregiverID",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CaregiverID",
                table: "Students");

            // Drop Caregivers table
            migrationBuilder.DropTable(
                name: "Caregivers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6e9ef90c-b4b4-402d-a998-fe7ba3dd19a1", "AQAAAAIAAYagAAAAEDPegFPVkUv6gAhQhX0f9oNqgt25xicHjB1Lcz8sxRt23h2rPc5imKsN8LlSff4IBQ==", "b69085cf-061b-4c22-853a-2081b6b2bb2c" });
        }
    }
}
