using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvondaleCollegeClinic.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHOMEROOMTEACHERRELATIONSHIP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homerooms_Teachers_TeacherID",
                table: "Homerooms");

            migrationBuilder.DropIndex(
                name: "IX_Homerooms_TeacherID",
                table: "Homerooms");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "28a04f25-60d9-491e-b6cc-4aca0664741b", "AQAAAAIAAYagAAAAEI1Wmf9yRxMd+SVmVsY0a/YTNSlElbLfQtsObkZL6BFoL4GOqSnaQCtSiG1CS7CaSA==", "d5bd2a6c-2555-4ac9-9308-3c3d80e59bef" });

            migrationBuilder.CreateIndex(
                name: "IX_Homerooms_TeacherID",
                table: "Homerooms",
                column: "TeacherID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Homerooms_Teachers_TeacherID",
                table: "Homerooms",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homerooms_Teachers_TeacherID",
                table: "Homerooms");

            migrationBuilder.DropIndex(
                name: "IX_Homerooms_TeacherID",
                table: "Homerooms");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6b99534e-8b7c-4844-ad16-df617b206951", "AQAAAAIAAYagAAAAEC/JGkqmQ40dJxG1ZMVyb92Ad7YX9jjx3NrfzBJnb5yICT8YQbSusGBhrqDwlkcxkA==", "5b8f3043-d7f9-439c-aabe-187fe560a1a9" });

            migrationBuilder.CreateIndex(
                name: "IX_Homerooms_TeacherID",
                table: "Homerooms",
                column: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Homerooms_Teachers_TeacherID",
                table: "Homerooms",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
