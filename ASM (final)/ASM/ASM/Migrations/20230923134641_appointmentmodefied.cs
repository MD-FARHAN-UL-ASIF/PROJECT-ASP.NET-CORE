using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Migrations
{
    /// <inheritdoc />
    public partial class appointmentmodefied : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Appointments");
        }
    }
}
