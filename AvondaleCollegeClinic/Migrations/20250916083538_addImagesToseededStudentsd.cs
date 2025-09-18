using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvondaleCollegeClinic.Migrations
{
    /// <inheritdoc />
    public partial class addImagesToseededStudentsd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eab28a37-1cd9-4696-942c-1aec6eaadc50", "AQAAAAIAAYagAAAAEAA9wzULS5mQxEgiz7+jv3lxPG62fY4pNs9JUjGL+1eo2/oiavrLaAD2MG7K1LUyNw==", "2e6bb199-2f05-450a-98d6-43c34e62d2af" });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250001",
                column: "ImagePath",
                value: "/images/students/1.jpg");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250002",
                column: "ImagePath",
                value: "/images/students/1.jpg");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250003",
                column: "ImagePath",
                value: "/images/students/1.jpg");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250004",
                column: "ImagePath",
                value: "/images/students/1.jpg");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250005",
                column: "ImagePath",
                value: "/images/students/1.jpg");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250006",
                column: "ImagePath",
                value: "/images/students/1.jpg");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250007",
                column: "ImagePath",
                value: "/images/students/1.jpg");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250008",
                column: "ImagePath",
                value: "/images/students/1.jpg");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250009",
                column: "ImagePath",
                value: "/images/students/1.jpg");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250010",
                column: "ImagePath",
                value: "/images/students/1.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "31ebd866-54d3-466f-a5d1-32af6be8b690", "AQAAAAIAAYagAAAAEKY/5FGGJmSepnOoHDNDqe/MyXIDg86v4edc3wZZ9dt/HTyyxC+qcVuIzi3U97l6+A==", "63ed24d9-979d-4d4d-99e0-060936df3a99" });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250001",
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250002",
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250003",
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250004",
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250005",
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250006",
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250007",
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250008",
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250009",
                column: "ImagePath",
                value: "");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250010",
                column: "ImagePath",
                value: "");
        }
    }
}
