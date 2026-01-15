using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MultiVendor_WebApiServer.Migrations
{
    /// <inheritdoc />
    public partial class seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandName", "CategoryId", "CreatedAt", "Description", "IsPublished", "Name", "UpdatedAt", "VendorId" },
                values: new object[] { new Guid("77777777-7777-7777-7777-777777777777"), "Samsung", new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 1, 4, 12, 0, 0, 0, DateTimeKind.Utc), "Latest Samsung flagship smartphone with high performance", true, "Galaxy S25", null, new Guid("22222222-2222-2222-2222-222222222222") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));
        }
    }
}
