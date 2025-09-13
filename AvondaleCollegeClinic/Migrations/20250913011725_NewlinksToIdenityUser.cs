using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvondaleCollegeClinic.Migrations
{
    /// <inheritdoc />
    public partial class NewlinksToIdenityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Teachers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Doctors",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Caregivers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarPath",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityOfBirth",
                table: "AspNetUsers",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "MustSetPassword",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserKind",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "00000000-0000-0000-0000-000000000010", null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AvatarPath", "CityOfBirth", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "MustSetPassword", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserKind", "UserName" },
                values: new object[] { "00000000-0000-0000-0000-000000000001", 0, null, null, "374f3635-75bf-4f8f-b36c-c0c151803b07", "admin@avondaleclinic.com", true, "System", "Admin", false, null, false, "ADMIN@AVONDALECLINIC.COM", "ADMIN@AVONDALECLINIC.COM", "AQAAAAIAAYagAAAAEB7P3Dn4v/lfW4Jjdj9DAoM20fhc2jOLnTZO0/HHiAvSulDqQA+sLN4yMWrKNBX3GA==", null, false, "d9ab70bd-ed24-44e8-8d6b-dcfce06ae9b8", false, 0, "admin@avondaleclinic.com" });

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 1,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 2,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 3,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 4,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 5,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 6,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 7,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 8,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 9,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 10,
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250001",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250002",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250003",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250004",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250005",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250006",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250007",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250008",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250009",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250010",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250001",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250002",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250003",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250004",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250005",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250006",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250007",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250008",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250009",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250010",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250001",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250002",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250003",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250004",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250005",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250006",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250007",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250008",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250009",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250010",
                column: "IdentityUserId",
                value: null);

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "00000000-0000-0000-0000-000000000010", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_IdentityUserId",
                table: "Teachers",
                column: "IdentityUserId",
                unique: true,
                filter: "[IdentityUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Students_IdentityUserId",
                table: "Students",
                column: "IdentityUserId",
                unique: true,
                filter: "[IdentityUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_IdentityUserId",
                table: "Doctors",
                column: "IdentityUserId",
                unique: true,
                filter: "[IdentityUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Caregivers_IdentityUserId",
                table: "Caregivers",
                column: "IdentityUserId",
                unique: true,
                filter: "[IdentityUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Caregivers_AspNetUsers_IdentityUserId",
                table: "Caregivers",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_AspNetUsers_IdentityUserId",
                table: "Doctors",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_IdentityUserId",
                table: "Students",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_AspNetUsers_IdentityUserId",
                table: "Teachers",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Caregivers_AspNetUsers_IdentityUserId",
                table: "Caregivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_AspNetUsers_IdentityUserId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_IdentityUserId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_AspNetUsers_IdentityUserId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_IdentityUserId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Students_IdentityUserId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_IdentityUserId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Caregivers_IdentityUserId",
                table: "Caregivers");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "00000000-0000-0000-0000-000000000010", "00000000-0000-0000-0000-000000000001" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000010");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Caregivers");

            migrationBuilder.DropColumn(
                name: "AvatarPath",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CityOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MustSetPassword",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserKind",
                table: "AspNetUsers");
        }
    }
}
