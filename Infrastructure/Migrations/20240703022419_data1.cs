using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class data1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEBfyFJQYozVBCWIyVkJvQv4xee10M4jQAckbMAWXqVHlKmJ3bakEu2rv4RVlEqAy+w==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEN/5SQc2hEG9ycOXIa335ev0mZR/Sx4iDjNOekRlvAoEzdJfA0GLAKJPIO9+yaCKYg==");

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "id",
                keyValue: 1,
                column: "addingDate",
                value: new DateTime(2024, 7, 3, 5, 24, 17, 880, DateTimeKind.Local).AddTicks(1519));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "id",
                keyValue: 2,
                column: "addingDate",
                value: new DateTime(2024, 7, 3, 5, 24, 17, 880, DateTimeKind.Local).AddTicks(1596));

            migrationBuilder.InsertData(
                table: "Governorates",
                columns: new[] { "id", "name", "status" },
                values: new object[] { 3, "Cairo", 1 });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2024, 7, 3, 5, 24, 18, 161, DateTimeKind.Local).AddTicks(6121));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIURBQkL9SD0+BHAZ1j9l48vK45F3DzGxPfmzzE7aeD+eC0/tgxVN7QcqeF5GB8c+Q==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMKiYwCXMJq0pYn6s+zpnIhYJMpXtzCWjJL/jkr5JyFDdEAct5RVEbVMc1cbrOddJQ==");

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "id",
                keyValue: 1,
                column: "addingDate",
                value: new DateTime(2024, 7, 3, 5, 20, 56, 146, DateTimeKind.Local).AddTicks(7834));

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "id",
                keyValue: 2,
                column: "addingDate",
                value: new DateTime(2024, 7, 3, 5, 20, 56, 146, DateTimeKind.Local).AddTicks(7908));

            migrationBuilder.InsertData(
                table: "Governorates",
                columns: new[] { "id", "name", "status" },
                values: new object[] { 1, "Cairo", 1 });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2024, 7, 3, 5, 20, 56, 394, DateTimeKind.Local).AddTicks(9437));
        }
    }
}
