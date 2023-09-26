using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Migrations
{
    /// <inheritdoc />
    public partial class ambulanceEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Ambulances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Ambulances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
