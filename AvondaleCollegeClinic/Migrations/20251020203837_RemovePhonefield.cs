using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvondaleCollegeClinic.Migrations
{
    /// <inheritdoc />
    public partial class RemovePhonefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Caregivers");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Teachers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Doctors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Caregivers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "18895624-0479-47bc-9773-b6810b862169", "AQAAAAIAAYagAAAAEFbFgy2knDjWdiLE2eCfTBSjh/w1w61pJo7bzAxfEK8zZwCIc47ujU0KS8vWkEF41g==", "e6f58734-1661-446c-92c1-17af0b0f1b09" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Caregivers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Caregivers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4c659892-568b-41ea-83e9-f77b6a87db12", "AQAAAAIAAYagAAAAEM0hT57UjL+EF/fcWtvdqajKypjZdZshqi2sBiIb+tKDBo2eksp/521t8BhQdshT1Q==", "00148619-0ca5-4b23-9ca2-74f357bb6f87" });

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250001",
                column: "Phone",
                value: "021-123-4567");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250002",
                column: "Phone",
                value: "022-987-6543");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250003",
                column: "Phone",
                value: "021-555-8899");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250004",
                column: "Phone",
                value: "021-333-2222");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250005",
                column: "Phone",
                value: "022-111-5555");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250006",
                column: "Phone",
                value: "021-456-7890");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250007",
                column: "Phone",
                value: "021-999-8888");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250008",
                column: "Phone",
                value: "022-123-9999");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250009",
                column: "Phone",
                value: "021-234-5678");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250010",
                column: "Phone",
                value: "022-777-3333");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250001",
                column: "Phone",
                value: "021-111-2345");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250002",
                column: "Phone",
                value: "021-222-3456");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250003",
                column: "Phone",
                value: "021-333-4567");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250004",
                column: "Phone",
                value: "021-444-5678");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250005",
                column: "Phone",
                value: "021-555-6789");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250006",
                column: "Phone",
                value: "021-666-7890");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250007",
                column: "Phone",
                value: "021-777-8901");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250008",
                column: "Phone",
                value: "021-888-9012");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250009",
                column: "Phone",
                value: "021-999-0123");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250010",
                column: "Phone",
                value: "021-000-1234");
        }
    }
}
