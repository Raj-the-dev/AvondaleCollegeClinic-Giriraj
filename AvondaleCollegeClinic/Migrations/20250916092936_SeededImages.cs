using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvondaleCollegeClinic.Migrations
{
    /// <inheritdoc />
    public partial class SeededImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "60f6540c-cfe5-4d61-bc53-75f028dcfb40", "AQAAAAIAAYagAAAAEDfEj/0bc/qSrheZFUrJPc4x68Eqt5MwOIQKu2YieKBBM+HDtjQXU4tz2VbWS7rMBQ==", "c8ce2505-b8bb-4dcd-9719-2ce443d93f66" });

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250001",
                column: "ImagePath",
                value: "/images/caregivers/1.jpeg");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250002",
                column: "ImagePath",
                value: "/images/caregivers/2.jpeg");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250003",
                column: "ImagePath",
                value: "/images/caregivers/3.jpeg");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250004",
                column: "ImagePath",
                value: "/images/caregivers/5.jepg.jpg");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250005",
                column: "ImagePath",
                value: "/images/caregivers/6.jpg");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250006",
                column: "ImagePath",
                value: "/images/caregivers/7.jpg");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250007",
                column: "ImagePath",
                value: "/images/caregivers/8.jpg");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250008",
                column: "ImagePath",
                value: "/images/caregivers/9.jpg");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250009",
                column: "ImagePath",
                value: "/images/caregivers/10.jpg");

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250010",
                column: "ImagePath",
                value: "/images/caregivers/11.jpg");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250001",
                column: "ImagePath",
                value: "/images/doctors/1.jpg");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250002",
                column: "ImagePath",
                value: "/images/doctors/2.jpg");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250003",
                column: "ImagePath",
                value: "/images/doctors/3.png");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250004",
                column: "ImagePath",
                value: "/images/doctors/4.png");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250005",
                column: "ImagePath",
                value: "/images/doctors/5.jpg");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250006",
                column: "ImagePath",
                value: "/images/doctors/6.png");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250007",
                column: "ImagePath",
                value: "/images/doctors/7.jpg");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250008",
                column: "ImagePath",
                value: "/images/doctors/8.jpg");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250009",
                column: "ImagePath",
                value: "/images/doctors/9.jpg");

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250010",
                column: "ImagePath",
                value: "/images/doctors/10.jpg");

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250001",
                columns: new[] { "Email", "FirstName", "ImagePath", "LastName" },
                values: new object[] { "v.Prasad@avondale.school.nz", "Mr. Vijay", "/images/teachers/MrPrasad.jpg", "Prasad" });

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250002",
                column: "ImagePath",
                value: "/images/teachers/Principle.jpeg");

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250003",
                column: "ImagePath",
                value: "/images/teachers/5.jpg");

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250004",
                column: "ImagePath",
                value: "/images/teachers/6.jpg");

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250005",
                column: "ImagePath",
                value: "/images/teachers/7.jpg");

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250006",
                column: "ImagePath",
                value: "/images/teachers/8.jpg");

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250007",
                column: "ImagePath",
                value: "/images/teachers/9.jpg");

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250008",
                column: "ImagePath",
                value: "/images/teachers/10.jpg");

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250009",
                column: "ImagePath",
                value: "/images/teachers/2.jpg");

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250010",
                column: "ImagePath",
                value: "/images/teachers/3.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "eab28a37-1cd9-4696-942c-1aec6eaadc50", "AQAAAAIAAYagAAAAEAA9wzULS5mQxEgiz7+jv3lxPG62fY4pNs9JUjGL+1eo2/oiavrLaAD2MG7K1LUyNw==", "2e6bb199-2f05-450a-98d6-43c34e62d2af" });

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250001",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250002",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250003",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250004",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250005",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250006",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250007",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250008",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250009",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: "acc250010",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250001",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250002",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250003",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250004",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250005",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250006",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250007",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250008",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250009",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250010",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250001",
                columns: new[] { "Email", "FirstName", "ImagePath", "LastName" },
                values: new object[] { "olivia.park@avondale.school.nz", "Olivia", null, "Park" });

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250002",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250003",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250004",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250005",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250006",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250007",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250008",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250009",
                column: "ImagePath",
                value: null);

            migrationBuilder.UpdateData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250010",
                column: "ImagePath",
                value: null);
        }
    }
}
