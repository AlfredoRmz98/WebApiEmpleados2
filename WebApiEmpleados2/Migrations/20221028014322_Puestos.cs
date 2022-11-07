using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiEmpleados2.Migrations
{
    /// <inheritdoc />
    public partial class Puestos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Puesto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puesto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmpleadoPuesto",
                columns: table => new
                {
                    empleadosId = table.Column<int>(type: "int", nullable: false),
                    puestosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpleadoPuesto", x => new { x.empleadosId, x.puestosId });
                    table.ForeignKey(
                        name: "FK_EmpleadoPuesto_Empleados_empleadosId",
                        column: x => x.empleadosId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpleadoPuesto_Puesto_puestosId",
                        column: x => x.puestosId,
                        principalTable: "Puesto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadoPuesto_puestosId",
                table: "EmpleadoPuesto",
                column: "puestosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpleadoPuesto");

            migrationBuilder.DropTable(
                name: "Puesto");
        }
    }
}
