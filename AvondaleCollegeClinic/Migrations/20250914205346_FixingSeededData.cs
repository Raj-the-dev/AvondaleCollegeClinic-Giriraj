using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AvondaleCollegeClinic.Migrations
{
    /// <inheritdoc />
    public partial class FixingSeededData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250001");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250002");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250003");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250004");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250005");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250006");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250007");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250008");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250009");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd-250010");

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250001");

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250002");

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250003");

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250004");

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250005");

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250006");

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250007");

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250008");

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250009");

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "StudentID",
                keyValue: "ac250010");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250001");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250002");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250003");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250004");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250005");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250006");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250007");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250008");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250009");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act-250010");

            migrationBuilder.DeleteData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Caregivers",
                keyColumn: "CaregiverID",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 1,
                column: "DoctorID",
                value: "acd250001");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 2,
                column: "DoctorID",
                value: "acd250002");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 3,
                column: "DoctorID",
                value: "acd250003");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 4,
                column: "DoctorID",
                value: "acd250004");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 5,
                column: "DoctorID",
                value: "acd250005");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 6,
                column: "DoctorID",
                value: "acd250006");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 7,
                column: "DoctorID",
                value: "acd250007");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 8,
                column: "DoctorID",
                value: "acd250008");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 9,
                column: "DoctorID",
                value: "acd250009");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 10,
                column: "DoctorID",
                value: "acd250010");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9e76af9b-af95-45be-a9c5-d5cea78fc65c", "AQAAAAIAAYagAAAAEF/bM8mAXANXCA6V592K9J0DhLHQTywMZbaBUY/jV7Ei47XYGEpas522a5ZkPvWLrA==", "10e361f7-9ea5-4b90-8604-e67ebc447ea5" });

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 1,
                column: "DoctorID",
                value: "acd250001");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 2,
                column: "DoctorID",
                value: "acd250002");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 3,
                column: "DoctorID",
                value: "acd250003");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 4,
                column: "DoctorID",
                value: "acd250004");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 5,
                column: "DoctorID",
                value: "acd250005");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 6,
                column: "DoctorID",
                value: "acd250006");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 7,
                column: "DoctorID",
                value: "acd250007");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 8,
                column: "DoctorID",
                value: "acd250008");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 9,
                column: "DoctorID",
                value: "acd250009");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 10,
                column: "DoctorID",
                value: "acd250010");

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorID", "Email", "FirstName", "IdentityUserId", "ImagePath", "LastName", "Phone", "Specialization" },
                values: new object[,]
                {
                    { "acd250001", "anna.roberts@avondaleclinic.com", "Anna", null, null, "Roberts", "021-111-2345", 0 },
                    { "acd250002", "ben.morris@avondaleclinic.com", "Ben", null, null, "Morris", "021-222-3456", 1 },
                    { "acd250003", "claire.nguyen@avondaleclinic.com", "Claire", null, null, "Nguyen", "021-333-4567", 2 },
                    { "acd250004", "david.chen@avondaleclinic.com", "David", null, null, "Chen", "021-444-5678", 3 },
                    { "acd250005", "ella.turner@avondaleclinic.com", "Ella", null, null, "Turner", "021-555-6789", 4 },
                    { "acd250006", "frank.white@avondaleclinic.com", "Frank", null, null, "White", "021-666-7890", 0 },
                    { "acd250007", "grace.lee@avondaleclinic.com", "Grace", null, null, "Lee", "021-777-8901", 1 },
                    { "acd250008", "harry.singh@avondaleclinic.com", "Harry", null, null, "Singh", "021-888-9012", 2 },
                    { "acd250009", "isla.walker@avondaleclinic.com", "Isla", null, null, "Walker", "021-999-0123", 3 },
                    { "acd250010", "jack.patel@avondaleclinic.com", "Jack", null, null, "Patel", "021-000-1234", 4 }
                });

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250001",
                column: "TeacherID",
                value: "act250001");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250002",
                column: "TeacherID",
                value: "act250002");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250003",
                column: "TeacherID",
                value: "act250003");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250004",
                column: "TeacherID",
                value: "act250004");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250005",
                column: "TeacherID",
                value: "act250005");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250006",
                column: "TeacherID",
                value: "act250006");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250007",
                column: "TeacherID",
                value: "act250007");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250008",
                column: "TeacherID",
                value: "act250008");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250009",
                column: "TeacherID",
                value: "act250009");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250010",
                column: "TeacherID",
                value: "act250010");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 1,
                column: "DoctorID",
                value: "acd250001");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 2,
                column: "DoctorID",
                value: "acd250002");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 3,
                column: "DoctorID",
                value: "acd250003");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 4,
                column: "DoctorID",
                value: "acd250004");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 5,
                column: "DoctorID",
                value: "acd250005");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 6,
                column: "DoctorID",
                value: "acd250006");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 7,
                column: "DoctorID",
                value: "acd250007");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 8,
                column: "DoctorID",
                value: "acd250008");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 9,
                column: "DoctorID",
                value: "acd250009");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 10,
                column: "DoctorID",
                value: "acd250010");

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "TeacherID", "Email", "FirstName", "IdentityUserId", "ImagePath", "LastName", "TeacherCode" },
                values: new object[,]
                {
                    { "act250001", "olivia.park@avondale.school.nz", "Olivia", null, null, "Park", "OPA" },
                    { "act250002", "james.ngata@avondale.school.nz", "James", null, null, "Ngata", "JNG" },
                    { "act250003", "sophia.lee@avondale.school.nz", "Sophia", null, null, "Lee", "SLE" },
                    { "act250004", "ethan.white@avondale.school.nz", "Ethan", null, null, "White", "EWH" },
                    { "act250005", "ava.singh@avondale.school.nz", "Ava", null, null, "Singh", "ASI" },
                    { "act250006", "william.morris@avondale.school.nz", "William", null, null, "Morris", "WMO" },
                    { "act250007", "isabella.tao@avondale.school.nz", "Isabella", null, null, "Tao", "ITA" },
                    { "act250008", "lucas.patel@avondale.school.nz", "Lucas", null, null, "Patel", "LPA" },
                    { "act250009", "emily.brown@avondale.school.nz", "Emily", null, null, "Brown", "EBR" },
                    { "act250010", "daniel.king@avondale.school.nz", "Daniel", null, null, "King", "DKG" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250001");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250002");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250003");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250004");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250005");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250006");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250007");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250008");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250009");

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250010");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250001");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250002");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250003");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250004");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250005");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250006");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250007");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250008");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250009");

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "TeacherID",
                keyValue: "act250010");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 1,
                column: "DoctorID",
                value: "acd-250001");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 2,
                column: "DoctorID",
                value: "acd-250002");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 3,
                column: "DoctorID",
                value: "acd-250003");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 4,
                column: "DoctorID",
                value: "acd-250004");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 5,
                column: "DoctorID",
                value: "acd-250005");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 6,
                column: "DoctorID",
                value: "acd-250006");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 7,
                column: "DoctorID",
                value: "acd-250007");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 8,
                column: "DoctorID",
                value: "acd-250008");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 9,
                column: "DoctorID",
                value: "acd-250009");

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 10,
                column: "DoctorID",
                value: "acd-250010");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ad230367-4ecc-40cd-839c-1da598075cb0", "AQAAAAIAAYagAAAAECc1iCqefTwGl/Ifpu/AMOJzx+S/0F0YPMOpUTZX5QpK1bQFr7te55MOxqN9zQps8w==", "a868b896-9b15-407c-8c1f-18e0eef03bd2" });

            migrationBuilder.InsertData(
                table: "Caregivers",
                columns: new[] { "CaregiverID", "DOB", "Email", "FirstName", "IdentityUserId", "ImagePath", "LastName", "Phone", "Relationship" },
                values: new object[,]
                {
                    { 1, new DateTime(1980, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "emma.wilson@email.com", "Emma", null, null, "Wilson", "021-123-4567", 0 },
                    { 2, new DateTime(1975, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.evans@email.com", "John", null, null, "Evans", "022-987-6543", 0 },
                    { 3, new DateTime(1985, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "maya.singh@email.com", "Maya", null, null, "Singh", "021-555-8899", 1 },
                    { 4, new DateTime(1979, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "chris.brown@email.com", "Chris", null, null, "Brown", "021-333-2222", 2 },
                    { 5, new DateTime(1982, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "sophie.nguyen@email.com", "Sophie", null, null, "Nguyen", "022-111-5555", 0 },
                    { 6, new DateTime(1973, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "liam.king@email.com", "Liam", null, null, "King", "021-456-7890", 3 },
                    { 7, new DateTime(1988, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "olivia.rao@email.com", "Olivia", null, null, "Rao", "021-999-8888", 0 },
                    { 8, new DateTime(1981, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "ethan.lee@email.com", "Ethan", null, null, "Lee", "022-123-9999", 1 },
                    { 9, new DateTime(1986, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "grace.taylor@email.com", "Grace", null, null, "Taylor", "021-234-5678", 0 },
                    { 10, new DateTime(1977, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "raj.patel@email.com", "Raj", null, null, "Patel", "022-777-3333", 3 }
                });

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 1,
                column: "DoctorID",
                value: "acd-250001");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 2,
                column: "DoctorID",
                value: "acd-250002");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 3,
                column: "DoctorID",
                value: "acd-250003");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 4,
                column: "DoctorID",
                value: "acd-250004");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 5,
                column: "DoctorID",
                value: "acd-250005");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 6,
                column: "DoctorID",
                value: "acd-250006");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 7,
                column: "DoctorID",
                value: "acd-250007");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 8,
                column: "DoctorID",
                value: "acd-250008");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 9,
                column: "DoctorID",
                value: "acd-250009");

            migrationBuilder.UpdateData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 10,
                column: "DoctorID",
                value: "acd-250010");

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorID", "Email", "FirstName", "IdentityUserId", "ImagePath", "LastName", "Phone", "Specialization" },
                values: new object[,]
                {
                    { "acd-250001", "anna.roberts@avondaleclinic.com", "Anna", null, null, "Roberts", "021-111-2345", 0 },
                    { "acd-250002", "ben.morris@avondaleclinic.com", "Ben", null, null, "Morris", "021-222-3456", 1 },
                    { "acd-250003", "claire.nguyen@avondaleclinic.com", "Claire", null, null, "Nguyen", "021-333-4567", 2 },
                    { "acd-250004", "david.chen@avondaleclinic.com", "David", null, null, "Chen", "021-444-5678", 3 },
                    { "acd-250005", "ella.turner@avondaleclinic.com", "Ella", null, null, "Turner", "021-555-6789", 4 },
                    { "acd-250006", "frank.white@avondaleclinic.com", "Frank", null, null, "White", "021-666-7890", 0 },
                    { "acd-250007", "grace.lee@avondaleclinic.com", "Grace", null, null, "Lee", "021-777-8901", 1 },
                    { "acd-250008", "harry.singh@avondaleclinic.com", "Harry", null, null, "Singh", "021-888-9012", 2 },
                    { "acd-250009", "isla.walker@avondaleclinic.com", "Isla", null, null, "Walker", "021-999-0123", 3 },
                    { "acd-250010", "jack.patel@avondaleclinic.com", "Jack", null, null, "Patel", "021-000-1234", 4 }
                });

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250001",
                column: "TeacherID",
                value: "act-250001");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250002",
                column: "TeacherID",
                value: "act-250002");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250003",
                column: "TeacherID",
                value: "act-250003");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250004",
                column: "TeacherID",
                value: "act-250004");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250005",
                column: "TeacherID",
                value: "act-250005");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250006",
                column: "TeacherID",
                value: "act-250006");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250007",
                column: "TeacherID",
                value: "act-250007");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250008",
                column: "TeacherID",
                value: "act-250008");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250009",
                column: "TeacherID",
                value: "act-250009");

            migrationBuilder.UpdateData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250010",
                column: "TeacherID",
                value: "act-250010");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 1,
                column: "DoctorID",
                value: "acd-250001");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 2,
                column: "DoctorID",
                value: "acd-250002");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 3,
                column: "DoctorID",
                value: "acd-250003");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 4,
                column: "DoctorID",
                value: "acd-250004");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 5,
                column: "DoctorID",
                value: "acd-250005");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 6,
                column: "DoctorID",
                value: "acd-250006");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 7,
                column: "DoctorID",
                value: "acd-250007");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 8,
                column: "DoctorID",
                value: "acd-250008");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 9,
                column: "DoctorID",
                value: "acd-250009");

            migrationBuilder.UpdateData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 10,
                column: "DoctorID",
                value: "acd-250010");

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "TeacherID", "Email", "FirstName", "IdentityUserId", "ImagePath", "LastName", "TeacherCode" },
                values: new object[,]
                {
                    { "act-250001", "olivia.park@avondale.school.nz", "Olivia", null, null, "Park", "OPA" },
                    { "act-250002", "james.ngata@avondale.school.nz", "James", null, null, "Ngata", "JNG" },
                    { "act-250003", "sophia.lee@avondale.school.nz", "Sophia", null, null, "Lee", "SLE" },
                    { "act-250004", "ethan.white@avondale.school.nz", "Ethan", null, null, "White", "EWH" },
                    { "act-250005", "ava.singh@avondale.school.nz", "Ava", null, null, "Singh", "ASI" },
                    { "act-250006", "william.morris@avondale.school.nz", "William", null, null, "Morris", "WMO" },
                    { "act-250007", "isabella.tao@avondale.school.nz", "Isabella", null, null, "Tao", "ITA" },
                    { "act-250008", "lucas.patel@avondale.school.nz", "Lucas", null, null, "Patel", "LPA" },
                    { "act-250009", "emily.brown@avondale.school.nz", "Emily", null, null, "Brown", "EBR" },
                    { "act-250010", "daniel.king@avondale.school.nz", "Daniel", null, null, "King", "DKG" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentID", "CaregiverID", "DOB", "Email", "FirstName", "HomeroomID", "IdentityUserId", "ImagePath", "LastName" },
                values: new object[,]
                {
                    { "ac250001", 3, new DateTime(2007, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "sam.hill@school.com", "Sam", "hr250001", null, "", "Hill" },
                    { "ac250002", 3, new DateTime(2008, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "lily.evans@school.com", "Lily", "hr250003", null, "", "Evans" },
                    { "ac250003", 3, new DateTime(2009, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "jake.smith@school.com", "Jake", "hr250003", null, "", "Smith" },
                    { "ac250004", 3, new DateTime(2007, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "emma.johnson@school.com", "Emma", "hr250001", null, "", "Johnson" },
                    { "ac250005", 4, new DateTime(2008, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "mia.brown@school.com", "Mia", "hr250003", null, "", "Brown" },
                    { "ac250006", 2, new DateTime(2009, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "noah.taylor@school.com", "Noah", "hr250002", null, "", "Taylor" },
                    { "ac250007", 1, new DateTime(2007, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "olivia.anderson@school.com", "Olivia", "hr250001", null, "", "Anderson" },
                    { "ac250008", 3, new DateTime(2008, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "liam.thomas@school.com", "Liam", "hr250001", null, "", "Thomas" },
                    { "ac250009", 5, new DateTime(2009, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "ava.jackson@school.com", "Ava", "hr250002", null, "", "Jackson" },
                    { "ac250010", 1, new DateTime(2007, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "ethan.white@school.com", "Ethan", "hr250002", null, "", "White" }
                });
        }
    }
}
