using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cs_project_amplo_dadosambientais.Migrations
{
    /// <inheritdoc />
    public partial class InsertAirQuality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "AirQuality",
            columns: new[] { "Id", "Data", "Obs", "InsertTimeStamp", "StationId" },
            values: new object[,] // INSERT INTO public."AirQuality"("Id", "Data", "Obs", "InsertTimeStamp", "StationId") VALUES(?, ?, ?, ?, ?);
            {
                { "537d2377-ae9e-472b-aae9-14582c1aaf91", "{ \"data\": \"\" }", "Error", DateTime.UtcNow, "6c1b6204-05d4-4bd2-8437-d070a9225124" },
                { "f74125ec-448d-44f6-a7c2-72a9fc77122e", "{ \"DioxidoDeEnxofre\": 5.66, \"MonoxidoDeCarbono\": 257.49, \"MonoxidoDeNitrogenio\": 4.75, \"DioxidoDeNitrogenio\": 5.75, \"OxidosDeNitrogenio\": 10.5, \"ParticulasInalaveis2\": 7.62 }", "Freqüência Horária com amostra 1 hora a 3,0 m", DateTime.UtcNow, "6c1b6204-05d4-4bd2-8437-d070a9225124" },
                { "925e52a0-fad6-41e5-b221-b164b671e524", "{ \"DioxidoDeEnxofre\": 5.13, \"MonoxidoDeCarbono\": 293.14, \"MonoxidoDeNitrogenio\": 4.81, \"DioxidoDeNitrogenio\": 8.62, \"OxidosDeNitrogenio\": 13.43, \"ParticulasInalaveis2\": 10.73 }", "Freqüência Horária com amostra 1 hora a 3,0 m", DateTime.UtcNow, "4bcda7a9-01c4-4724-a3d8-c07b6ffb8d29" },
                { "3d6af425-4ee4-415a-87f1-5cfc21cddee7", "{ \"PartículasTotaisEmSuspensao\": 24.06, \"PartículasInalaveis10\": 24.81, \"DioxidoDeEnxofre\": 8.49, \"Monóxido de Nitrogênio\": 1.63, \"DioxidoDeNitrogenio\": 7.52, \"OxidosDeNitrogenio\": 9.15, \"MonoxidoDeCarbono\": 1106.4, \"PartículasInalaveis2\": 19.23, \"Ozonio\": 32.57 }", "Freqüência Horária com amostra 1 hora a 3,0 m", DateTime.UtcNow, "50749e1e-56be-48ee-b24a-31142a33974d" },
                { "64db4d3c-65a9-4922-9f9c-77622ece95f5", null, "Error", DateTime.UtcNow, "50749e1e-56be-48ee-b24a-31142a33974d" },
            }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM AirQuality;");
        }
    }
}
