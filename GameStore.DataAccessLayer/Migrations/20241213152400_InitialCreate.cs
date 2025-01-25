using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStore.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("3a44173f-af26-44d4-8d9a-1dea4deece28"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("56383a14-67b6-4e39-bd95-7fe5ab4cd4a1"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("964308d8-ee99-4183-8a7c-7250b0dc4025"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("a8cb8a48-9341-45d5-9935-b5e85a6a12c9"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("c0829af7-bc76-426d-876f-c46b8bf2747b"));

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "Id",
                keyValue: new Guid("9eaf69d3-e3cd-4d26-a046-c3ab2d5890ba"));

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "Id",
                keyValue: new Guid("a4b873e6-f4d9-4e4a-86d5-4c9e0c43e0c6"));

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "Id",
                keyValue: new Guid("b130be97-1b2f-4dc0-aec6-8b880fe8de52"));

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "Id",
                keyValue: new Guid("c7e0c86a-5578-4eb3-84e7-6356a6ad967f"));

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: new Guid("03313169-1015-42ef-bc16-eaec561e2f24"));

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: new Guid("497db7cf-a033-4315-bfa3-2ae7813a4bcf"));

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: new Guid("4f01980f-c728-4fb7-815a-198c8e030002"));

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name", "ParentGenreId" },
                values: new object[,]
                {
                    { new Guid("1331f6a0-8d53-4251-9543-8d46e81160e1"), "Strategy", null },
                    { new Guid("57e8a8ad-a7db-425d-bcc7-f4309a4b2679"), "RPG", null },
                    { new Guid("99cfd312-2cb3-4296-a64b-4be651c00857"), "Action", null },
                    { new Guid("9d786fbe-2c2e-4177-911e-ccd9eca00a6b"), "Sports", null },
                    { new Guid("b925dc8a-6945-445b-8c5d-37c2b971cb8a"), "Adventure", null }
                });

            migrationBuilder.InsertData(
                table: "Platforms",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { new Guid("2b7d46fa-a679-4149-ac3a-0b7a839dc07f"), "PC" },
                    { new Guid("4612e101-6ff0-414f-96c9-d749dea4034f"), "Xbox" },
                    { new Guid("5bc33d9d-f191-4d5c-8528-78ad01aabd2d"), "Nintendo Switch" },
                    { new Guid("83795d51-85e3-4426-af02-ee9502b3b10d"), "PlayStation" }
                });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "Id", "CompanyName", "Description", "HomePage" },
                values: new object[,]
                {
                    { new Guid("08f98468-7e86-4901-a9b3-0046971697ed"), "EA", "Popular for sports games.", "https://www.ea.com" },
                    { new Guid("466149ee-a9d8-41da-847f-29255b8765da"), "Ubisoft", "A leading publisher in video games.", "https://www.ubisoft.com" },
                    { new Guid("840f794d-6b66-436e-8abf-e0cd9557875e"), "Nintendo", "Famous for the Mario franchise.", "https://www.nintendo.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("1331f6a0-8d53-4251-9543-8d46e81160e1"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("57e8a8ad-a7db-425d-bcc7-f4309a4b2679"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("99cfd312-2cb3-4296-a64b-4be651c00857"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("9d786fbe-2c2e-4177-911e-ccd9eca00a6b"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("b925dc8a-6945-445b-8c5d-37c2b971cb8a"));

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "Id",
                keyValue: new Guid("2b7d46fa-a679-4149-ac3a-0b7a839dc07f"));

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "Id",
                keyValue: new Guid("4612e101-6ff0-414f-96c9-d749dea4034f"));

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "Id",
                keyValue: new Guid("5bc33d9d-f191-4d5c-8528-78ad01aabd2d"));

            migrationBuilder.DeleteData(
                table: "Platforms",
                keyColumn: "Id",
                keyValue: new Guid("83795d51-85e3-4426-af02-ee9502b3b10d"));

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: new Guid("08f98468-7e86-4901-a9b3-0046971697ed"));

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: new Guid("466149ee-a9d8-41da-847f-29255b8765da"));

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: new Guid("840f794d-6b66-436e-8abf-e0cd9557875e"));

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name", "ParentGenreId" },
                values: new object[,]
                {
                    { new Guid("3a44173f-af26-44d4-8d9a-1dea4deece28"), "Adventure", null },
                    { new Guid("56383a14-67b6-4e39-bd95-7fe5ab4cd4a1"), "Action", null },
                    { new Guid("964308d8-ee99-4183-8a7c-7250b0dc4025"), "Sports", null },
                    { new Guid("a8cb8a48-9341-45d5-9935-b5e85a6a12c9"), "Strategy", null },
                    { new Guid("c0829af7-bc76-426d-876f-c46b8bf2747b"), "RPG", null }
                });

            migrationBuilder.InsertData(
                table: "Platforms",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { new Guid("9eaf69d3-e3cd-4d26-a046-c3ab2d5890ba"), "PC" },
                    { new Guid("a4b873e6-f4d9-4e4a-86d5-4c9e0c43e0c6"), "PlayStation" },
                    { new Guid("b130be97-1b2f-4dc0-aec6-8b880fe8de52"), "Nintendo Switch" },
                    { new Guid("c7e0c86a-5578-4eb3-84e7-6356a6ad967f"), "Xbox" }
                });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "Id", "CompanyName", "Description", "HomePage" },
                values: new object[,]
                {
                    { new Guid("03313169-1015-42ef-bc16-eaec561e2f24"), "Ubisoft", "A leading publisher in video games.", "https://www.ubisoft.com" },
                    { new Guid("497db7cf-a033-4315-bfa3-2ae7813a4bcf"), "EA", "Popular for sports games.", "https://www.ea.com" },
                    { new Guid("4f01980f-c728-4fb7-815a-198c8e030002"), "Nintendo", "Famous for the Mario franchise.", "https://www.nintendo.com" }
                });
        }
    }
}
