using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Migrations
{
    /// <inheritdoc />
    public partial class patientAndDoctorTableAltered : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "City",
                table: "Patients",
                newName: "Location");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Doctors",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Fee",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Loacation",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Loacation",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Doctors");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Patients",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Doctors",
                newName: "City");
        }
    }
}
