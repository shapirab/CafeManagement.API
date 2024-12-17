using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixedPOEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "PurchaseOrders",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "PurchaseOrders",
                newName: "Comments");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "PurchaseOrders",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PurchaseOrders",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "PurchaseOrders",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Comments",
                table: "PurchaseOrders",
                newName: "Description");
        }
    }
}
