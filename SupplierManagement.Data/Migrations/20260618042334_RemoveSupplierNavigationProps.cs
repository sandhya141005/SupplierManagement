using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupplierManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSupplierNavigationProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Cities_CityId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Countries_CountryId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_States_StateId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_CityId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_CountryId",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_StateId",
                table: "Suppliers");

            migrationBuilder.AlterColumn<string>(
                name: "ContactNo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ContactNo",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CityId",
                table: "Suppliers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CountryId",
                table: "Suppliers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_StateId",
                table: "Suppliers",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Cities_CityId",
                table: "Suppliers",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Countries_CountryId",
                table: "Suppliers",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_States_StateId",
                table: "Suppliers",
                column: "StateId",
                principalTable: "States",
                principalColumn: "StateId");
        }
    }
}
