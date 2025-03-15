using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cs_project_amplo_dadosambientais.Migrations
{
    /// <inheritdoc />
    public partial class InsertStations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Station",
                columns: new[] { "Id", "Name", "Obs", "InsertTimeStamp" },
                values: new object[,]
                {
                    { "6c1b6204-05d4-4bd2-8437-d070a9225124", "EMQAr-Sul 3 - Guanabara", "Obs1", DateTime.UtcNow },
                    { "4bcda7a9-01c4-4724-a3d8-c07b6ffb8d29", "EMQAr-Sul 4 - Belo Horizonte", "Obs2" , DateTime.UtcNow },
                    { "50749e1e-56be-48ee-b24a-31142a33974d", "EMQAr-Sul 5 - Mãe-Bá", null, DateTime.UtcNow }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Station");
        }
    }
}
