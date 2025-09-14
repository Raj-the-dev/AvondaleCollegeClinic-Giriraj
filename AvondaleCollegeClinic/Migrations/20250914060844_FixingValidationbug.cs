using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvondaleCollegeClinic.Migrations
{
    /// <inheritdoc />
    public partial class FixingValidationbug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teacher_Email",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teacher_FullName",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Student_Email",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Student_FullName",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Doctor_Email",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctor_FullName",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Caregiver_Email",
                table: "Caregivers");

            migrationBuilder.DropIndex(
                name: "IX_Caregiver_FullName",
                table: "Caregivers");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Caregivers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ad230367-4ecc-40cd-839c-1da598075cb0", "AQAAAAIAAYagAAAAECc1iCqefTwGl/Ifpu/AMOJzx+S/0F0YPMOpUTZX5QpK1bQFr7te55MOxqN9zQps8w==", "a868b896-9b15-407c-8c1f-18e0eef03bd2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Teachers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Doctors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Caregivers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "41828aba-241f-4de5-ae76-808493b64791", "AQAAAAIAAYagAAAAEH2SkbEuFEfrtvtIhEf0dFHKG8O51pXrlMHD6uMCGp08GCsX1/b1Dl2HcGijY4FfkA==", "1f8a62ae-4049-497c-aa87-0d5b5a5fb45f" });

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_Email",
                table: "Teachers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teacher_FullName",
                table: "Teachers",
                columns: new[] { "FirstName", "LastName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_Email",
                table: "Students",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_FullName",
                table: "Students",
                columns: new[] { "FirstName", "LastName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_Email",
                table: "Doctors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_FullName",
                table: "Doctors",
                columns: new[] { "FirstName", "LastName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Caregiver_Email",
                table: "Caregivers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Caregiver_FullName",
                table: "Caregivers",
                columns: new[] { "FirstName", "LastName" },
                unique: true);
        }
    }
}
