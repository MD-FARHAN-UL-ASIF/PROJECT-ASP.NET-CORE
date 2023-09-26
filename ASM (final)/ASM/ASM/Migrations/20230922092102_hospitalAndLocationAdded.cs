using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM.Migrations
{
    /// <inheritdoc />
    public partial class hospitalAndLocationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Hospital",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Loacation",
                table: "Doctors");

            migrationBuilder.AddColumn<int>(
                name: "HospitalId",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hospitals_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_HospitalId",
                table: "Doctors",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_LocationId",
                table: "Hospitals",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Hospitals_HospitalId",
                table: "Doctors",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Hospitals_HospitalId",
                table: "Doctors");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_HospitalId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "Doctors");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Hospital",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Loacation",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
