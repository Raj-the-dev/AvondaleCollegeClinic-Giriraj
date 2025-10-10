using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AvondaleCollegeClinic.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCaregiverandStudentrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Caregivers_CaregiverID",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_CaregiverID",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CaregiverID",
                table: "Students");

            migrationBuilder.CreateTable(
                name: "StudentCaregivers",
                columns: table => new
                {
                    StudentID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CaregiverID = table.Column<string>(type: "nvarchar(9)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCaregivers", x => new { x.StudentID, x.CaregiverID });
                    table.ForeignKey(
                        name: "FK_StudentCaregivers_Caregiver",
                        column: x => x.CaregiverID,
                        principalTable: "Caregivers",
                        principalColumn: "CaregiverID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCaregivers_Student",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9efd74d1-8c6d-41c1-9dfc-64696c69cd79", "AQAAAAIAAYagAAAAEMaS3HZTsYGuEobqldB7qRHTDfa26KOAqkRbq7h+t+DYjxaBPl4jiLh9CD9FqQYG2A==", "a4886354-b400-4eec-a72d-e097f74f93f0" });

            migrationBuilder.InsertData(
                table: "StudentCaregivers",
                columns: new[] { "CaregiverID", "StudentID" },
                values: new object[,]
                {
                    { "acc250001", "ac250001" },
                    { "acc250001", "ac250002" },
                    { "acc250001", "ac250003" },
                    { "acc250001", "ac250004" },
                    { "acc250004", "ac250005" },
                    { "acc250002", "ac250006" },
                    { "acc250001", "ac250007" },
                    { "acc250001", "ac250008" },
                    { "acc250005", "ac250009" },
                    { "acc250001", "ac250010" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentCaregivers_CaregiverID",
                table: "StudentCaregivers",
                column: "CaregiverID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentCaregivers");

            migrationBuilder.AddColumn<string>(
                name: "CaregiverID",
                table: "Students",
                type: "nvarchar(9)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "28a04f25-60d9-491e-b6cc-4aca0664741b", "AQAAAAIAAYagAAAAEI1Wmf9yRxMd+SVmVsY0a/YTNSlElbLfQtsObkZL6BFoL4GOqSnaQCtSiG1CS7CaSA==", "d5bd2a6c-2555-4ac9-9308-3c3d80e59bef" });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250001",
                column: "CaregiverID",
                value: "acc250001");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250002",
                column: "CaregiverID",
                value: "acc250001");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250003",
                column: "CaregiverID",
                value: "acc250001");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250004",
                column: "CaregiverID",
                value: "acc250001");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250005",
                column: "CaregiverID",
                value: "acc250004");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250006",
                column: "CaregiverID",
                value: "acc250002");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250007",
                column: "CaregiverID",
                value: "acc250001");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250008",
                column: "CaregiverID",
                value: "acc250001");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250009",
                column: "CaregiverID",
                value: "acc250005");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250010",
                column: "CaregiverID",
                value: "acc250001");

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
        }
    }
}
