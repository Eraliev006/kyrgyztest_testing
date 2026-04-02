using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KyrgyzTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedComputers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Computers",
                columns: new[] { "Id", "LastHeartbeat", "StationNumber", "Status" },
                values: new object[,]
                {
                    { new Guid("0894d936-3f7c-4337-a8e6-236185e7212b"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3780), 18, 0 },
                    { new Guid("09bf6a62-7543-4460-8c63-bd5eea2e1d4f"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3790), 20, 0 },
                    { new Guid("119c3af1-0e4a-473e-bc03-7cd535b8ced3"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3850), 35, 0 },
                    { new Guid("174054f7-1964-4992-a672-87b0c1ac54a2"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3890), 43, 0 },
                    { new Guid("1b0b37e1-b3ef-4d63-86aa-fcc70065bf67"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3800), 23, 0 },
                    { new Guid("1bcf3b03-d107-4a96-9124-f8462f8f64bc"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3860), 37, 0 },
                    { new Guid("25cd2d26-b39b-4ada-ac15-64d3116b8af3"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3900), 47, 0 },
                    { new Guid("3159a778-e840-4e9d-a561-e5ec52154fae"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3770), 14, 0 },
                    { new Guid("40ff5319-f529-4458-98d3-be1cd06c3925"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3750), 11, 0 },
                    { new Guid("48530fb0-b06f-45dc-a0c0-1ce5c9427c36"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3710), 1, 0 },
                    { new Guid("486ef804-3780-4f79-81e8-e307e02d0788"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3840), 31, 0 },
                    { new Guid("53c365ce-63d3-4745-a899-5f51557e0ab0"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3900), 46, 0 },
                    { new Guid("5f8b02ae-1b85-4e27-9b81-c9c9d23e40d6"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3880), 42, 0 },
                    { new Guid("6b117c0d-c818-4aec-96a0-a0318f8fabc9"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3750), 10, 0 },
                    { new Guid("6de178e6-2209-43ab-9494-e966d8e8acf9"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3790), 19, 0 },
                    { new Guid("6e97b051-2e3a-4341-b595-74dc1ae85300"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3800), 22, 0 },
                    { new Guid("73b800d9-802b-470e-ab50-3adc5c0c5c1f"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3850), 34, 0 },
                    { new Guid("7d449d40-67c5-4d67-8201-ccf2b595b554"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3740), 8, 0 },
                    { new Guid("85b2107c-1902-4bd2-811a-c4746ac20584"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3810), 24, 0 },
                    { new Guid("892feff9-8f1c-424f-9a32-24f031cecffc"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3910), 49, 0 },
                    { new Guid("8b7b62dd-8d67-47a4-914b-39101d16a3e4"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3790), 21, 0 },
                    { new Guid("8da6a542-036e-4c86-9a63-30ea9aa42e6a"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3760), 13, 0 },
                    { new Guid("98321269-30df-4ca8-b9a3-2d435635a4be"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3880), 41, 0 },
                    { new Guid("98a56a6d-611f-4181-8da2-895085f6ca51"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3910), 50, 0 },
                    { new Guid("9b33c93b-d47b-441a-ad9a-0a3db6c6d6d1"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3770), 16, 0 },
                    { new Guid("9e5a5735-f7bd-4883-aeb8-6eb08c2d289f"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3710), 2, 0 },
                    { new Guid("a456e769-2ab9-4784-8c25-18e654cbc545"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3820), 27, 0 },
                    { new Guid("a523cff8-3967-4d08-969b-8f4b5a1ac088"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3730), 5, 0 },
                    { new Guid("a98f4b3d-160b-4be5-8666-d5fafb463671"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3780), 17, 0 },
                    { new Guid("af4e0403-fb86-41c0-b67a-8bb5bb1be40f"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3890), 45, 0 },
                    { new Guid("b0fb4ae7-da42-425d-85ce-8a02ccddf35b"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3740), 7, 0 },
                    { new Guid("b15cec1a-7065-4e25-8a55-6bdb24d57257"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3830), 30, 0 },
                    { new Guid("b2cdd43b-7285-4e40-8fba-3d631f78c9bf"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3770), 15, 0 },
                    { new Guid("b3f2cfeb-c6fc-485c-a6de-fcca0382de84"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3730), 6, 0 },
                    { new Guid("b59a5a81-a726-415a-b9cd-71a8485a865f"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3830), 28, 0 },
                    { new Guid("bb69c071-9e72-46c2-8623-7a1fb6372bd3"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3760), 12, 0 },
                    { new Guid("bf496b19-41ff-4c69-8c2c-5e94fdf19255"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3840), 32, 0 },
                    { new Guid("c370a9ed-df45-4cb8-ba12-945bbf885d13"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3870), 39, 0 },
                    { new Guid("c650e9ae-45bb-4b7f-a8c1-3aa0257295f3"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3870), 40, 0 },
                    { new Guid("cef15816-d11a-43a3-afa6-012e560386f3"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3720), 3, 0 },
                    { new Guid("d0cfe79b-e94d-4e01-8c2a-fe0aeb2d185c"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3830), 29, 0 },
                    { new Guid("e0ea45ab-2a0f-486a-8c11-c51d28f5bcba"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3870), 38, 0 },
                    { new Guid("e226fd0a-efcd-4aca-b3c5-982a94312752"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3740), 9, 0 },
                    { new Guid("e4ca973f-d656-4e70-9dd5-2f5d6befa84d"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3850), 33, 0 },
                    { new Guid("ea84578d-6c5f-4cf1-b510-109ee40eeafb"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3720), 4, 0 },
                    { new Guid("eac3e130-f79f-4d39-879e-8ee8bc085676"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3810), 26, 0 },
                    { new Guid("eb713330-dfde-43d0-80a1-6387f2b2f53b"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3910), 48, 0 },
                    { new Guid("ecbbbd5d-9ace-4f9e-a94e-dbfca26b0e1b"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3860), 36, 0 },
                    { new Guid("f25ee7d5-fe6d-4c67-9daf-1aad69164910"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3810), 25, 0 },
                    { new Guid("fe1a9d8a-80c3-476f-8306-f0afbcb33a3b"), new DateTime(2026, 4, 2, 21, 28, 48, 775, DateTimeKind.Utc).AddTicks(3890), 44, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("0894d936-3f7c-4337-a8e6-236185e7212b"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("09bf6a62-7543-4460-8c63-bd5eea2e1d4f"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("119c3af1-0e4a-473e-bc03-7cd535b8ced3"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("174054f7-1964-4992-a672-87b0c1ac54a2"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("1b0b37e1-b3ef-4d63-86aa-fcc70065bf67"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("1bcf3b03-d107-4a96-9124-f8462f8f64bc"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("25cd2d26-b39b-4ada-ac15-64d3116b8af3"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("3159a778-e840-4e9d-a561-e5ec52154fae"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("40ff5319-f529-4458-98d3-be1cd06c3925"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("48530fb0-b06f-45dc-a0c0-1ce5c9427c36"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("486ef804-3780-4f79-81e8-e307e02d0788"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("53c365ce-63d3-4745-a899-5f51557e0ab0"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("5f8b02ae-1b85-4e27-9b81-c9c9d23e40d6"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("6b117c0d-c818-4aec-96a0-a0318f8fabc9"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("6de178e6-2209-43ab-9494-e966d8e8acf9"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("6e97b051-2e3a-4341-b595-74dc1ae85300"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("73b800d9-802b-470e-ab50-3adc5c0c5c1f"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("7d449d40-67c5-4d67-8201-ccf2b595b554"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("85b2107c-1902-4bd2-811a-c4746ac20584"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("892feff9-8f1c-424f-9a32-24f031cecffc"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("8b7b62dd-8d67-47a4-914b-39101d16a3e4"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("8da6a542-036e-4c86-9a63-30ea9aa42e6a"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("98321269-30df-4ca8-b9a3-2d435635a4be"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("98a56a6d-611f-4181-8da2-895085f6ca51"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("9b33c93b-d47b-441a-ad9a-0a3db6c6d6d1"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("9e5a5735-f7bd-4883-aeb8-6eb08c2d289f"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("a456e769-2ab9-4784-8c25-18e654cbc545"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("a523cff8-3967-4d08-969b-8f4b5a1ac088"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("a98f4b3d-160b-4be5-8666-d5fafb463671"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("af4e0403-fb86-41c0-b67a-8bb5bb1be40f"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("b0fb4ae7-da42-425d-85ce-8a02ccddf35b"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("b15cec1a-7065-4e25-8a55-6bdb24d57257"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("b2cdd43b-7285-4e40-8fba-3d631f78c9bf"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("b3f2cfeb-c6fc-485c-a6de-fcca0382de84"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("b59a5a81-a726-415a-b9cd-71a8485a865f"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("bb69c071-9e72-46c2-8623-7a1fb6372bd3"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("bf496b19-41ff-4c69-8c2c-5e94fdf19255"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("c370a9ed-df45-4cb8-ba12-945bbf885d13"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("c650e9ae-45bb-4b7f-a8c1-3aa0257295f3"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("cef15816-d11a-43a3-afa6-012e560386f3"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("d0cfe79b-e94d-4e01-8c2a-fe0aeb2d185c"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("e0ea45ab-2a0f-486a-8c11-c51d28f5bcba"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("e226fd0a-efcd-4aca-b3c5-982a94312752"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("e4ca973f-d656-4e70-9dd5-2f5d6befa84d"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("ea84578d-6c5f-4cf1-b510-109ee40eeafb"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("eac3e130-f79f-4d39-879e-8ee8bc085676"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("eb713330-dfde-43d0-80a1-6387f2b2f53b"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("ecbbbd5d-9ace-4f9e-a94e-dbfca26b0e1b"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("f25ee7d5-fe6d-4c67-9daf-1aad69164910"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("fe1a9d8a-80c3-476f-8306-f0afbcb33a3b"));
        }
    }
}
