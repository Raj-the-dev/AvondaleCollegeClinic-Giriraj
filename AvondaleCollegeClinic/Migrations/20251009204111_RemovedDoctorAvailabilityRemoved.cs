using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AvondaleCollegeClinic.Migrations
{
    /// <inheritdoc />
    public partial class RemovedDoctorAvailabilityRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorAvailabilities");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DailyEndTime",
                table: "Doctors",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DailyStartTime",
                table: "Doctors",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "SlotMinutes",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "WorksFri",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WorksMon",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WorksSat",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WorksSun",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WorksThu",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WorksTue",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WorksWed",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4c659892-568b-41ea-83e9-f77b6a87db12", "AQAAAAIAAYagAAAAEM0hT57UjL+EF/fcWtvdqajKypjZdZshqi2sBiIb+tKDBo2eksp/521t8BhQdshT1Q==", "00148619-0ca5-4b23-9ca2-74f357bb6f87" });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250001",
                columns: new[] { "DailyEndTime", "DailyStartTime", "SlotMinutes", "WorksFri", "WorksMon", "WorksSat", "WorksSun", "WorksThu", "WorksTue", "WorksWed" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), 30, true, true, false, false, true, true, true });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250002",
                columns: new[] { "DailyEndTime", "DailyStartTime", "SlotMinutes", "WorksFri", "WorksMon", "WorksSat", "WorksSun", "WorksThu", "WorksTue", "WorksWed" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), 30, true, true, false, false, true, true, true });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250003",
                columns: new[] { "DailyEndTime", "DailyStartTime", "SlotMinutes", "WorksFri", "WorksMon", "WorksSat", "WorksSun", "WorksThu", "WorksTue", "WorksWed" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), 30, true, true, false, false, true, true, true });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250004",
                columns: new[] { "DailyEndTime", "DailyStartTime", "SlotMinutes", "WorksFri", "WorksMon", "WorksSat", "WorksSun", "WorksThu", "WorksTue", "WorksWed" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), 30, true, true, false, false, true, true, true });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250005",
                columns: new[] { "DailyEndTime", "DailyStartTime", "SlotMinutes", "WorksFri", "WorksMon", "WorksSat", "WorksSun", "WorksThu", "WorksTue", "WorksWed" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), 30, true, true, false, false, true, true, true });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250006",
                columns: new[] { "DailyEndTime", "DailyStartTime", "SlotMinutes", "WorksFri", "WorksMon", "WorksSat", "WorksSun", "WorksThu", "WorksTue", "WorksWed" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), 30, true, true, false, false, true, true, true });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250007",
                columns: new[] { "DailyEndTime", "DailyStartTime", "SlotMinutes", "WorksFri", "WorksMon", "WorksSat", "WorksSun", "WorksThu", "WorksTue", "WorksWed" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), 30, true, true, false, false, true, true, true });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250008",
                columns: new[] { "DailyEndTime", "DailyStartTime", "SlotMinutes", "WorksFri", "WorksMon", "WorksSat", "WorksSun", "WorksThu", "WorksTue", "WorksWed" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), 30, true, true, false, false, true, true, true });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250009",
                columns: new[] { "DailyEndTime", "DailyStartTime", "SlotMinutes", "WorksFri", "WorksMon", "WorksSat", "WorksSun", "WorksThu", "WorksTue", "WorksWed" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), 30, true, true, false, false, true, true, true });

            migrationBuilder.UpdateData(
                table: "Doctors",
                keyColumn: "DoctorID",
                keyValue: "acd250010",
                columns: new[] { "DailyEndTime", "DailyStartTime", "SlotMinutes", "WorksFri", "WorksMon", "WorksSat", "WorksSun", "WorksThu", "WorksTue", "WorksWed" },
                values: new object[] { new TimeSpan(0, 17, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0), 30, true, true, false, false, true, true, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyEndTime",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DailyStartTime",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "SlotMinutes",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "WorksFri",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "WorksMon",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "WorksSat",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "WorksSun",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "WorksThu",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "WorksTue",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "WorksWed",
                table: "Doctors");

            migrationBuilder.CreateTable(
                name: "DoctorAvailabilities",
                columns: table => new
                {
                    DoctorAvailabilityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorID = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    AvailableDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorAvailabilities", x => x.DoctorAvailabilityID);
                    table.ForeignKey(
                        name: "FK_DoctorAvailabilities_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-0000-0000-0000-000000000001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9efd74d1-8c6d-41c1-9dfc-64696c69cd79", "AQAAAAIAAYagAAAAEMaS3HZTsYGuEobqldB7qRHTDfa26KOAqkRbq7h+t+DYjxaBPl4jiLh9CD9FqQYG2A==", "a4886354-b400-4eec-a72d-e097f74f93f0" });

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

            migrationBuilder.CreateIndex(
                name: "IX_DoctorAvailabilities_DoctorID",
                table: "DoctorAvailabilities",
                column: "DoctorID");
        }
    }
}
