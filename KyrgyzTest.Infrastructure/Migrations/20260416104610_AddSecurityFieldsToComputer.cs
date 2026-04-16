using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KyrgyzTest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSecurityFieldsToComputer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "StationNumber",
                table: "ExamSessions");

            migrationBuilder.AddColumn<Guid>(
                name: "ComputerId",
                table: "ExamSessions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "StationNumber",
                table: "Computers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "Computers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceSecretHash",
                table: "Computers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsMainPool",
                table: "Computers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrusted",
                table: "Computers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastIpAddress",
                table: "Computers",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Computers",
                columns: new[] { "Id", "DeviceId", "DeviceSecretHash", "IsMainPool", "IsTrusted", "LastHeartbeat", "LastIpAddress", "StationNumber", "Status" },
                values: new object[,]
                {
                    { new Guid("00c1d166-44f9-446b-bf2d-702d46843634"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7540), null, 24, 0 },
                    { new Guid("05481edc-1e2c-4700-a086-18dfea8acf6b"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7510), null, 14, 0 },
                    { new Guid("07767f6d-5fae-48c9-9b85-5cd8e6fab741"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7470), null, 3, 0 },
                    { new Guid("0b2b0d70-fb63-4992-8f9c-afcaeca60fa8"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7590), null, 39, 0 },
                    { new Guid("0c272dc1-1dc6-4bfc-9338-beb806548184"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7500), null, 13, 0 },
                    { new Guid("107eba04-bb01-4ab6-9be6-9b4efd869e6f"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7600), null, 42, 0 },
                    { new Guid("22440ce4-ea15-4c94-84ad-5343255cff1b"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7550), null, 27, 0 },
                    { new Guid("24babd9d-f88c-495a-acbc-3ef015c391ad"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7560), null, 30, 0 },
                    { new Guid("2920255a-dfd8-4248-b937-da1fc4d6f687"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7530), null, 22, 0 },
                    { new Guid("2af02d70-f150-49fc-8d00-65c68c308e64"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7500), null, 11, 0 },
                    { new Guid("32da59f8-ed4e-49cc-90c5-25e491654157"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7590), null, 37, 0 },
                    { new Guid("382f097f-e97f-4d8b-81c9-1809bd95ccd8"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7600), null, 41, 0 },
                    { new Guid("3bb10183-e43f-4dcb-83b1-7444fb96767b"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7570), null, 32, 0 },
                    { new Guid("403979bd-4aaa-4d5d-9e56-9a644f3a55b5"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7510), null, 16, 0 },
                    { new Guid("41722276-13ce-43da-9f35-eae1d09859b5"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7570), null, 31, 0 },
                    { new Guid("471047e7-7d5c-479e-86ad-091a93094eda"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7610), null, 44, 0 },
                    { new Guid("4f687bde-54db-4189-8efe-837f0f12faef"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7580), null, 36, 0 },
                    { new Guid("5a6ef0fc-e215-43eb-a8a0-fad391b2b1a4"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7520), null, 17, 0 },
                    { new Guid("5d947698-16f1-4aed-a73c-d370346a1435"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7530), null, 20, 0 },
                    { new Guid("63021852-f379-48fd-8f14-1a610b4dd8dc"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7530), null, 21, 0 },
                    { new Guid("63a66dbf-7ed2-4333-b546-a5a68dd870ab"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7620), null, 47, 0 },
                    { new Guid("67342984-8b0a-4095-a75f-5655f8dce992"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7610), null, 45, 0 },
                    { new Guid("786bd44a-15a8-4dd2-8533-b8fa46fee4a3"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7620), null, 48, 0 },
                    { new Guid("82fb6a8a-fb92-4a14-980c-54ec7d604de7"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7600), null, 40, 0 },
                    { new Guid("864fda37-c77d-4ac1-b50d-2b7aa6372eb1"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7550), null, 25, 0 },
                    { new Guid("8a7e7bde-6b5c-4439-b8e6-aeb31abb0b28"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7570), null, 33, 0 },
                    { new Guid("8abe891b-5cd4-46f6-83bd-e66a0571344e"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7490), null, 9, 0 },
                    { new Guid("8c4b37d3-75b0-4879-a775-d241892516c6"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7540), null, 23, 0 },
                    { new Guid("8d0edf00-bc83-427f-a0e9-8576fe2c0d2e"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7480), null, 6, 0 },
                    { new Guid("8fc79fb9-90aa-4392-ad95-eaae70c09467"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7580), null, 35, 0 },
                    { new Guid("8fcf5eae-8d8c-42f8-b052-942be81b6df2"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7620), null, 46, 0 },
                    { new Guid("91d800ef-15f6-45ab-93b1-3f795bb75838"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7560), null, 29, 0 },
                    { new Guid("97e4fc02-465e-4e39-ae4f-41932cd9f80e"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7590), null, 38, 0 },
                    { new Guid("a636c34c-b97f-4a7e-8c93-cabc2f9b5e7b"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7460), null, 2, 0 },
                    { new Guid("af5a11de-ad4b-4dc1-8e53-7c7c026751ee"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7480), null, 7, 0 },
                    { new Guid("b28f36eb-7f14-4ccc-9387-383b343c7fa4"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7630), null, 50, 0 },
                    { new Guid("bca6ca12-f958-4fe2-953c-dc4f39874d1a"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7490), null, 8, 0 },
                    { new Guid("bffa810e-5db6-44e0-b41d-27b325efcdb8"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7520), null, 19, 0 },
                    { new Guid("c04a49fd-24fd-4bc0-931a-e3283a6c49f7"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7510), null, 15, 0 },
                    { new Guid("c7206f87-3fed-4a10-b4af-0e8b077d0af4"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7470), null, 4, 0 },
                    { new Guid("cb210115-f973-4a51-8669-58fd26ee043b"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7460), null, 1, 0 },
                    { new Guid("db8c41ed-a631-49ee-ae05-29cf8d6c1f1b"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7470), null, 5, 0 },
                    { new Guid("dd677986-c1f2-4368-bc88-3fec3fe344e8"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7630), null, 49, 0 },
                    { new Guid("de65d5b7-e6ca-4154-91e0-10b4317b5d60"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7610), null, 43, 0 },
                    { new Guid("e2d2f647-3d02-4ece-ab77-de07340e9dcd"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7490), null, 10, 0 },
                    { new Guid("e5643ca7-1082-4d82-8f8f-94234ab8311a"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7550), null, 26, 0 },
                    { new Guid("eb900a1f-11a8-4889-aa70-8a600c3b97c4"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7500), null, 12, 0 },
                    { new Guid("ecd06967-041d-472d-b39d-aba994686ff7"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7560), null, 28, 0 },
                    { new Guid("faf5afdb-c04b-4b55-8674-9de25438ddbc"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7580), null, 34, 0 },
                    { new Guid("ff1d34df-9ae6-4eae-9a63-80a0930c565a"), "", "", false, false, new DateTime(2026, 4, 16, 10, 46, 10, 255, DateTimeKind.Utc).AddTicks(7520), null, 18, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("00c1d166-44f9-446b-bf2d-702d46843634"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("05481edc-1e2c-4700-a086-18dfea8acf6b"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("07767f6d-5fae-48c9-9b85-5cd8e6fab741"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("0b2b0d70-fb63-4992-8f9c-afcaeca60fa8"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("0c272dc1-1dc6-4bfc-9338-beb806548184"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("107eba04-bb01-4ab6-9be6-9b4efd869e6f"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("22440ce4-ea15-4c94-84ad-5343255cff1b"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("24babd9d-f88c-495a-acbc-3ef015c391ad"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("2920255a-dfd8-4248-b937-da1fc4d6f687"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("2af02d70-f150-49fc-8d00-65c68c308e64"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("32da59f8-ed4e-49cc-90c5-25e491654157"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("382f097f-e97f-4d8b-81c9-1809bd95ccd8"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("3bb10183-e43f-4dcb-83b1-7444fb96767b"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("403979bd-4aaa-4d5d-9e56-9a644f3a55b5"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("41722276-13ce-43da-9f35-eae1d09859b5"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("471047e7-7d5c-479e-86ad-091a93094eda"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("4f687bde-54db-4189-8efe-837f0f12faef"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("5a6ef0fc-e215-43eb-a8a0-fad391b2b1a4"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("5d947698-16f1-4aed-a73c-d370346a1435"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("63021852-f379-48fd-8f14-1a610b4dd8dc"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("63a66dbf-7ed2-4333-b546-a5a68dd870ab"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("67342984-8b0a-4095-a75f-5655f8dce992"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("786bd44a-15a8-4dd2-8533-b8fa46fee4a3"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("82fb6a8a-fb92-4a14-980c-54ec7d604de7"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("864fda37-c77d-4ac1-b50d-2b7aa6372eb1"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("8a7e7bde-6b5c-4439-b8e6-aeb31abb0b28"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("8abe891b-5cd4-46f6-83bd-e66a0571344e"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("8c4b37d3-75b0-4879-a775-d241892516c6"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("8d0edf00-bc83-427f-a0e9-8576fe2c0d2e"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("8fc79fb9-90aa-4392-ad95-eaae70c09467"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("8fcf5eae-8d8c-42f8-b052-942be81b6df2"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("91d800ef-15f6-45ab-93b1-3f795bb75838"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("97e4fc02-465e-4e39-ae4f-41932cd9f80e"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("a636c34c-b97f-4a7e-8c93-cabc2f9b5e7b"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("af5a11de-ad4b-4dc1-8e53-7c7c026751ee"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("b28f36eb-7f14-4ccc-9387-383b343c7fa4"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("bca6ca12-f958-4fe2-953c-dc4f39874d1a"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("bffa810e-5db6-44e0-b41d-27b325efcdb8"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("c04a49fd-24fd-4bc0-931a-e3283a6c49f7"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("c7206f87-3fed-4a10-b4af-0e8b077d0af4"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("cb210115-f973-4a51-8669-58fd26ee043b"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("db8c41ed-a631-49ee-ae05-29cf8d6c1f1b"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("dd677986-c1f2-4368-bc88-3fec3fe344e8"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("de65d5b7-e6ca-4154-91e0-10b4317b5d60"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("e2d2f647-3d02-4ece-ab77-de07340e9dcd"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("e5643ca7-1082-4d82-8f8f-94234ab8311a"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("eb900a1f-11a8-4889-aa70-8a600c3b97c4"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("ecd06967-041d-472d-b39d-aba994686ff7"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("faf5afdb-c04b-4b55-8674-9de25438ddbc"));

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: new Guid("ff1d34df-9ae6-4eae-9a63-80a0930c565a"));

            migrationBuilder.DropColumn(
                name: "ComputerId",
                table: "ExamSessions");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Computers");

            migrationBuilder.DropColumn(
                name: "DeviceSecretHash",
                table: "Computers");

            migrationBuilder.DropColumn(
                name: "IsMainPool",
                table: "Computers");

            migrationBuilder.DropColumn(
                name: "IsTrusted",
                table: "Computers");

            migrationBuilder.DropColumn(
                name: "LastIpAddress",
                table: "Computers");

            migrationBuilder.AddColumn<int>(
                name: "StationNumber",
                table: "ExamSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "StationNumber",
                table: "Computers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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
    }
}
