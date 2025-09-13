using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvondaleCollegeClinic.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLinksToIdenittyUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // CAREGIVERS
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Caregivers_AspNetUsers_IdentityUserId')
                    ALTER TABLE [Caregivers] DROP CONSTRAINT [FK_Caregivers_AspNetUsers_IdentityUserId];
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Caregivers_IdentityUserId')
                    DROP INDEX [IX_Caregivers_IdentityUserId] ON [Caregivers];
                IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IdentityUserId' AND Object_ID = Object_ID(N'Caregivers'))
                    ALTER TABLE [Caregivers] DROP COLUMN [IdentityUserId];
            ");

            // DOCTORS
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Doctors_AspNetUsers_IdentityUserId')
                    ALTER TABLE [Doctors] DROP CONSTRAINT [FK_Doctors_AspNetUsers_IdentityUserId];
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Doctors_IdentityUserId')
                    DROP INDEX [IX_Doctors_IdentityUserId] ON [Doctors];
                IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IdentityUserId' AND Object_ID = Object_ID(N'Doctors'))
                    ALTER TABLE [Doctors] DROP COLUMN [IdentityUserId];
            ");

            // STUDENTS
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Students_AspNetUsers_IdentityUserId')
                    ALTER TABLE [Students] DROP CONSTRAINT [FK_Students_AspNetUsers_IdentityUserId];
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Students_IdentityUserId')
                    DROP INDEX [IX_Students_IdentityUserId] ON [Students];
                IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IdentityUserId' AND Object_ID = Object_ID(N'Students'))
                    ALTER TABLE [Students] DROP COLUMN [IdentityUserId];
            ");

            // TEACHERS
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Teachers_AspNetUsers_IdentityUserId')
                    ALTER TABLE [Teachers] DROP CONSTRAINT [FK_Teachers_AspNetUsers_IdentityUserId];
                IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Teachers_IdentityUserId')
                    DROP INDEX [IX_Teachers_IdentityUserId] ON [Teachers];
                IF EXISTS (SELECT 1 FROM sys.columns WHERE Name = N'IdentityUserId' AND Object_ID = Object_ID(N'Teachers'))
                    ALTER TABLE [Teachers] DROP COLUMN [IdentityUserId];
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
