using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvondaleCollegeClinic.Migrations
{
    /// <inheritdoc />
    public partial class SeededNewPDFFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6b99534e-8b7c-4844-ad16-df617b206951", "AQAAAAIAAYagAAAAEC/JGkqmQ40dJxG1ZMVyb92Ad7YX9jjx3NrfzBJnb5yICT8YQbSusGBhrqDwlkcxkA==", "5b8f3043-d7f9-439c-aabe-187fe560a1a9" });

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 1,
                column: "File",
                value: "/labtests/labtestsample.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 2,
                column: "File",
                value: "/labtests/labtestsample.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 3,
                column: "File",
                value: "/labtests/labtestsample.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 4,
                column: "File",
                value: "/labtests/labtestsample.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 5,
                column: "File",
                value: "/labtests/labtestsample.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 6,
                column: "File",
                value: "/labtests/labtestsample.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 7,
                column: "File",
                value: "/labtests/labtestsample.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 8,
                column: "File",
                value: "/labtests/labtestsample.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 9,
                column: "File",
                value: "/labtests/labtestsample.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 10,
                column: "File",
                value: "/labtests/labtestsample.pdf");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "60f6540c-cfe5-4d61-bc53-75f028dcfb40", "AQAAAAIAAYagAAAAEDfEj/0bc/qSrheZFUrJPc4x68Eqt5MwOIQKu2YieKBBM+HDtjQXU4tz2VbWS7rMBQ==", "c8ce2505-b8bb-4dcd-9719-2ce443d93f66" });

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 1,
                column: "File",
                value: "report1.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 2,
                column: "File",
                value: "report2.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 3,
                column: "File",
                value: "report3.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 4,
                column: "File",
                value: "report4.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 5,
                column: "File",
                value: "report5.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 6,
                column: "File",
                value: "report6.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 7,
                column: "File",
                value: "report7.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 8,
                column: "File",
                value: "report8.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 9,
                column: "File",
                value: "report9.pdf");

            migrationBuilder.UpdateData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 10,
                column: "File",
                value: "report10.pdf");
        }
    }
}
