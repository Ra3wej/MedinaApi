using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedinaApi.Migrations
{
    /// <inheritdoc />
    public partial class addRecentTreatMents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecentTreatments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    HospitalId = table.Column<int>(type: "int", nullable: false),
                    ChronicDiasesId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecentTreatments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecentTreatments_ChronicDiases_ChronicDiasesId",
                        column: x => x.ChronicDiasesId,
                        principalTable: "ChronicDiases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecentTreatments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecentTreatments_Hospital_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospital",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecentTreatments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecentTreatments_ChronicDiasesId",
                table: "RecentTreatments",
                column: "ChronicDiasesId");

            migrationBuilder.CreateIndex(
                name: "IX_RecentTreatments_DoctorId",
                table: "RecentTreatments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_RecentTreatments_HospitalId",
                table: "RecentTreatments",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_RecentTreatments_PatientId",
                table: "RecentTreatments",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecentTreatments");
        }
    }
}
