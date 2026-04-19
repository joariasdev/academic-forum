using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademicForum.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAttendeeRecordProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasAttented",
                table: "AttendeeRecords",
                newName: "HasAttended");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasAttended",
                table: "AttendeeRecords",
                newName: "HasAttented");
        }
    }
}
