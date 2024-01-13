using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedinaApi.Migrations
{
    /// <inheritdoc />
    public partial class diseases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChronicDiases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiasesName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChronicDiases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientChronicDiases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChronicDiaseId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientChronicDiases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientChronicDiases_ChronicDiases_ChronicDiaseId",
                        column: x => x.ChronicDiaseId,
                        principalTable: "ChronicDiases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PatientChronicDiases_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientChronicDiases_ChronicDiaseId",
                table: "PatientChronicDiases",
                column: "ChronicDiaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientChronicDiases_PatientId",
                table: "PatientChronicDiases",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientChronicDiases");

            migrationBuilder.DropTable(
                name: "ChronicDiases");
        }
    }
}
