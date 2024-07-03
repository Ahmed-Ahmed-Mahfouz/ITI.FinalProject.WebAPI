using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Merchants_MerchantId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialPackages_Merchants_MerchantId",
                table: "SpecialPackages");

            migrationBuilder.DropTable(
                name: "Merchants");

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                table: "Orders",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MerchantPayingPercentageForRejectedOrders",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SpecialPickupShippingCost",
                table: "AspNetUsers",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoreName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Governorates",
                columns: new[] { "id", "name", "status" },
                values: new object[,]
                {
                    { 13, "Cairo", 1 },
                    { 2, "Giza", 1 }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "id", "governorateId", "name", "normalShippingCost", "pickupShippingCost", "status" },
                values: new object[,]
                {
                    { 1, 34, "Nasr City", 10m, 5m, 1 },
                    { 2, 2, "6th of October", 15m, 7m, 1 }
                });

            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "id", "addingDate", "cityId", "name", "status" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 7, 3, 5, 20, 56, 146, DateTimeKind.Local).AddTicks(7834), 1, "Nasr City Branch", 1 },
                    { 2, new DateTime(2024, 7, 3, 5, 20, 56, 146, DateTimeKind.Local).AddTicks(7908), 2, "6th of October Branch", 1 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "BranchId", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNo", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TwoFactorEnabled", "UserName", "UserType" },
                values: new object[] { "1", 0, "123 Main St", 1, "", "ApplicationUser", "johndoe@example.com", true, "John Doe", true, null, "JOHNDOE@EXAMPLE.COM", "JOHNDOE", "AQAAAAIAAYagAAAAEIURBQkL9SD0+BHAZ1j9l48vK45F3DzGxPfmzzE7aeD+eC0/tgxVN7QcqeF5GB8c+Q==", "1234567890", "1234567890", true, "", 1, false, "johndoe", 2 });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "BranchId", "CityId", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FullName", "GovernorateId", "LockoutEnabled", "LockoutEnd", "MerchantPayingPercentageForRejectedOrders", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNo", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "SpecialPickupShippingCost", "Status", "StoreName", "TwoFactorEnabled", "UserName", "UserType", "userId" },
                values: new object[] { "2", 0, "456 Main St", 2, 1, "", "Merchant", "janedoe@example.com", true, "Jane Doe", 1, true, null, 10m, "JANEDOE@EXAMPLE.COM", "JANEDOE", "AQAAAAIAAYagAAAAEMKiYwCXMJq0pYn6s+zpnIhYJMpXtzCWjJL/jkr5JyFDdEAct5RVEbVMc1cbrOddJQ==", "0987654321", "0987654321", true, "", 5m, 1, "Jane's Store", false, "janedoe", 1, null });

            migrationBuilder.InsertData(
                table: "Employees",
                column: "userId",
                value: "1");

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "BranchId", "CityId", "ClientName", "Date", "Email", "GovernorateId", "MerchantId", "Notes", "OrderMoneyReceived", "PaymentType", "Phone", "Phone2", "RepresentativeId", "ShippingCost", "ShippingId", "ShippingMoneyReceived", "ShippingToVillage", "Status", "Type", "VillageAndStreet" },
                values: new object[] { 1, 1, 1, "Client 1", new DateTime(2024, 7, 3, 5, 20, 56, 394, DateTimeKind.Local).AddTicks(9437), null, 1, "2", null, null, 1, "1234567890", null, "1", 10m, 1, null, false, 1, 1, "Village 1, Street 1" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CityId",
                table: "AspNetUsers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GovernorateId",
                table: "AspNetUsers",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_userId",
                table: "AspNetUsers",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_userId",
                table: "AspNetUsers",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cities_CityId",
                table: "AspNetUsers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Governorates_GovernorateId",
                table: "AspNetUsers",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_MerchantId",
                table: "Orders",
                column: "MerchantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialPackages_AspNetUsers_MerchantId",
                table: "SpecialPackages",
                column: "MerchantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_userId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cities_CityId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Governorates_GovernorateId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_MerchantId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialPackages_AspNetUsers_MerchantId",
                table: "SpecialPackages");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CityId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GovernorateId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_userId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "userId",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MerchantPayingPercentageForRejectedOrders",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpecialPickupShippingCost",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StoreName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    GovernorateId = table.Column<int>(type: "int", nullable: true),
                    MerchantPayingPercentageForRejectedOrders = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SpecialPickupShippingCost = table.Column<decimal>(type: "money", nullable: true),
                    StoreName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.userId);
                    table.ForeignKey(
                        name: "FK_Merchants_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Merchants_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Merchants_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_CityId",
                table: "Merchants",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_GovernorateId",
                table: "Merchants",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Merchants_MerchantId",
                table: "Orders",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialPackages_Merchants_MerchantId",
                table: "SpecialPackages",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "userId");
        }
    }
}
