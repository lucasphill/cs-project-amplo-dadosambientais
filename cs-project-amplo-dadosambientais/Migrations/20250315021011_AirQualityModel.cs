using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cs_project_amplo_dadosambientais.Migrations
{
    /// <inheritdoc />
    public partial class AirQualityModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirQuality",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Data = table.Column<string>(type: "jsonb", nullable: true),
                    Obs = table.Column<string>(type: "text", nullable: true),
                    InsertTimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirQuality", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirQuality_Station_StationId",
                        column: x => x.StationId,
                        principalTable: "Station",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirQuality_StationId",
                table: "AirQuality",
                column: "StationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirQuality");
        }
    }
}
