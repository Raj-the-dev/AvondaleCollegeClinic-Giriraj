using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AvondaleCollegeClinic.Migrations
{
    /// <inheritdoc />
    public partial class FixingSeededData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "DoctorAvailabilities",
                keyColumn: "DoctorAvailabilityID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250001");

            migrationBuilder.DeleteData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250002");

            migrationBuilder.DeleteData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250003");

            migrationBuilder.DeleteData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250004");

            migrationBuilder.DeleteData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250005");

            migrationBuilder.DeleteData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250006");

            migrationBuilder.DeleteData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250007");

            migrationBuilder.DeleteData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250008");

            migrationBuilder.DeleteData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250009");

            migrationBuilder.DeleteData(
                table: "Homerooms",
                keyColumn: "HomeroomID",
                keyValue: "hr250010");

            migrationBuilder.DeleteData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "LabTests",
                keyColumn: "LabtestID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "PrescriptionID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "PrescriptionID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "PrescriptionID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "PrescriptionID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "PrescriptionID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "PrescriptionID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "PrescriptionID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "PrescriptionID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "PrescriptionID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Prescriptions",
                keyColumn: "PrescriptionID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Diagnoses",
                keyColumn: "DiagnosisID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Diagnoses",
                keyColumn: "DiagnosisID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Diagnoses",
                keyColumn: "DiagnosisID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Diagnoses",
                keyColumn: "DiagnosisID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Diagnoses",
                keyColumn: "DiagnosisID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Diagnoses",
                keyColumn: "DiagnosisID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Diagnoses",
                keyColumn: "DiagnosisID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Diagnoses",
                keyColumn: "DiagnosisID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Diagnoses",
                keyColumn: "DiagnosisID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Diagnoses",
                keyColumn: "DiagnosisID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MedicalRecords",
                keyColumn: "MedicalRecordID",
                keyValue: 10);

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

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Appointments",
                keyColumn: "AppointmentID",
                keyValue: 10);

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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7f5cf42a-0aa5-4806-b7f3-d0a13d234af3", "AQAAAAIAAYagAAAAEH5lxsotX0GstYUp/g2HyxPMkCVhwRcfnuVBCnAwI2DxWvpu13KoUlpfLns86kURaA==", "07aa4c09-ef79-4335-9d94-c48f78e7649a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f288b714-2311-4b04-940d-4f45bb2abc3e", "AQAAAAIAAYagAAAAEM9asEIHXk4OcNzyD3ToZlkgzjB4h8ygTHZ470PU1IEcfiaDI/Zm/qilOvMrkpbSCw==", "7e33a2f8-2d86-498e-8490-0fc4129dc874" });

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

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "AppointmentID", "AppointmentDateTime", "DoctorID", "Reason", "Status", "StudentID" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 15, 10, 30, 0, 0, DateTimeKind.Unspecified), "acd250001", "General check-up", 1, "ac250001" },
                    { 2, new DateTime(2025, 4, 16, 11, 0, 0, 0, DateTimeKind.Unspecified), "acd250002", "Follow-up on injury", 0, "ac250002" },
                    { 3, new DateTime(2025, 4, 17, 9, 15, 0, 0, DateTimeKind.Unspecified), "acd250003", "Routine blood test", 3, "ac250003" },
                    { 4, new DateTime(2025, 4, 18, 14, 0, 0, 0, DateTimeKind.Unspecified), "acd250004", "Skin allergy consultation", 1, "ac250004" },
                    { 5, new DateTime(2025, 4, 19, 15, 30, 0, 0, DateTimeKind.Unspecified), "acd250005", "Sore throat", 2, "ac250005" },
                    { 6, new DateTime(2025, 4, 20, 13, 0, 0, 0, DateTimeKind.Unspecified), "acd250006", "Mental health support", 0, "ac250006" },
                    { 7, new DateTime(2025, 4, 21, 10, 45, 0, 0, DateTimeKind.Unspecified), "acd250007", "Back pain evaluation", 1, "ac250007" },
                    { 8, new DateTime(2025, 4, 22, 9, 0, 0, 0, DateTimeKind.Unspecified), "acd250008", "Yearly physical", 1, "ac250008" },
                    { 9, new DateTime(2025, 4, 23, 11, 30, 0, 0, DateTimeKind.Unspecified), "acd250009", "Follow-up on flu", 3, "ac250009" },
                    { 10, new DateTime(2025, 4, 24, 12, 15, 0, 0, DateTimeKind.Unspecified), "acd250010", "Foot injury assessment", 0, "ac250010" }
                });

            migrationBuilder.InsertData(
                table: "DoctorAvailabilities",
                columns: new[] { "DoctorAvailabilityID", "AvailableDate", "DoctorID", "EndTime", "StartTime" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250001", new DateTime(2025, 4, 15, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 15, 9, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2025, 4, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250002", new DateTime(2025, 4, 16, 13, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 16, 10, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2025, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250003", new DateTime(2025, 4, 17, 11, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 17, 8, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250004", new DateTime(2025, 4, 18, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 18, 13, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250005", new DateTime(2025, 4, 19, 12, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 19, 9, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250006", new DateTime(2025, 4, 20, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 20, 14, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(2025, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250007", new DateTime(2025, 4, 21, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 21, 11, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250008", new DateTime(2025, 4, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 22, 15, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, new DateTime(2025, 4, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250009", new DateTime(2025, 4, 23, 11, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 23, 8, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, new DateTime(2025, 4, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250010", new DateTime(2025, 4, 24, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 24, 12, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Homerooms",
                columns: new[] { "HomeroomID", "Block", "ClassNumber", "TeacherID", "YearLevel" },
                values: new object[,]
                {
                    { "hr250001", 0, 1, "act250001", 9 },
                    { "hr250002", 1, 2, "act250002", 10 },
                    { "hr250003", 2, 3, "act250003", 11 },
                    { "hr250004", 3, 4, "act250004", 12 },
                    { "hr250005", 4, 5, "act250005", 13 },
                    { "hr250006", 5, 6, "act250006", 9 },
                    { "hr250007", 0, 7, "act250007", 10 },
                    { "hr250008", 1, 8, "act250008", 11 },
                    { "hr250009", 2, 9, "act250009", 12 },
                    { "hr250010", 3, 10, "act250010", 13 }
                });

            migrationBuilder.InsertData(
                table: "MedicalRecords",
                columns: new[] { "MedicalRecordID", "Date", "DoctorID", "Notes", "StudentID" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250001", "Routine annual check-up. No issues found.", "ac250001" },
                    { 2, new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250002", "Reviewed knee sprain. Advised rest and light exercises.", "ac250002" },
                    { 3, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250003", "Blood test completed. Results within normal range.", "ac250003" },
                    { 4, new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250004", "Observed rash on arms. Prescribed topical cream.", "ac250004" },
                    { 5, new DateTime(2025, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250005", "Reported headache and fatigue. Referred for lab tests.", "ac250005" },
                    { 6, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250006", "Initial consultation for anxiety symptoms. Scheduled follow-up.", "ac250006" },
                    { 7, new DateTime(2025, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250007", "Complaints of lower back pain. Stretching advised.", "ac250007" },
                    { 8, new DateTime(2025, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250008", "Regular check-up completed. Advised hydration.", "ac250008" },
                    { 9, new DateTime(2025, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250009", "Follow-up after flu recovery. Patient doing well.", "ac250009" },
                    { 10, new DateTime(2025, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "acd250010", "Foot strain reviewed. Rest and ice recommended.", "ac250010" }
                });

            migrationBuilder.InsertData(
                table: "Diagnoses",
                columns: new[] { "DiagnosisID", "AppointmentID", "DateDiagnosed", "Description" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mild allergic reaction to pollen" },
                    { 2, 2, new DateTime(2024, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sprained ankle during soccer" },
                    { 3, 3, new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seasonal flu symptoms" },
                    { 4, 4, new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mild asthma attack" },
                    { 5, 5, new DateTime(2024, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Concussion evaluation after PE" },
                    { 6, 6, new DateTime(2024, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stomach virus" },
                    { 7, 7, new DateTime(2024, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sinus infection" },
                    { 8, 8, new DateTime(2024, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Minor burn on hand" },
                    { 9, 9, new DateTime(2024, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stress-related headaches" },
                    { 10, 10, new DateTime(2024, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rash caused by detergent allergy" }
                });

            migrationBuilder.InsertData(
                table: "LabTests",
                columns: new[] { "LabtestID", "File", "RecordID", "ResultDate", "TestType" },
                values: new object[,]
                {
                    { 1, "report1.pdf", 1, new DateOnly(2024, 10, 11), "Blood Test 1" },
                    { 2, "report2.pdf", 2, new DateOnly(2024, 10, 12), "Blood Test 2" },
                    { 3, "report3.pdf", 3, new DateOnly(2024, 10, 13), "Blood Test 3" },
                    { 4, "report4.pdf", 4, new DateOnly(2024, 10, 14), "Blood Test 4" },
                    { 5, "report5.pdf", 5, new DateOnly(2024, 10, 15), "Blood Test 5" },
                    { 6, "report6.pdf", 6, new DateOnly(2024, 10, 16), "Blood Test 6" },
                    { 7, "report7.pdf", 7, new DateOnly(2024, 10, 17), "Blood Test 7" },
                    { 8, "report8.pdf", 8, new DateOnly(2024, 10, 18), "Blood Test 8" },
                    { 9, "report9.pdf", 9, new DateOnly(2024, 10, 19), "Blood Test 9" },
                    { 10, "report10.pdf", 10, new DateOnly(2024, 10, 20), "Blood Test 10" }
                });

            migrationBuilder.InsertData(
                table: "Prescriptions",
                columns: new[] { "PrescriptionID", "DiagnosisID", "Dosage", "EndDate", "Medication", "StartDate" },
                values: new object[,]
                {
                    { 1, 1, "10mg once daily", new DateTime(2024, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cetirizine", new DateTime(2024, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2, "200mg every 6 hours", new DateTime(2024, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ibuprofen", new DateTime(2024, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, "500mg every 4-6 hours", new DateTime(2024, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paracetamol", new DateTime(2024, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 4, "2 puffs as needed", new DateTime(2024, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Albuterol Inhaler", new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 5, "As needed", new DateTime(2024, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rest and hydration", new DateTime(2024, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 6, "2mg after each loose stool", new DateTime(2024, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Loperamide", new DateTime(2024, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 7, "500mg three times daily", new DateTime(2024, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Amoxicillin", new DateTime(2024, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 8, "Apply twice daily", new DateTime(2024, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aloe Vera Gel", new DateTime(2024, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 9, "200mg twice daily", new DateTime(2024, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ibuprofen", new DateTime(2024, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 10, "Apply once daily", new DateTime(2024, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hydrocortisone cream", new DateTime(2024, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }
    }
}
