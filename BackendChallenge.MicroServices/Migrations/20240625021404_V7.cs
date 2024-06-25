using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendChallenge.MicroServices.Migrations
{
    /// <inheritdoc />
    public partial class V7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsEntity_Order_OrderEntityOrderId",
                schema: "BackendChallenge",
                table: "ProductsEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsEntity",
                schema: "BackendChallenge",
                table: "ProductsEntity");

            migrationBuilder.RenameTable(
                name: "ProductsEntity",
                schema: "BackendChallenge",
                newName: "Product",
                newSchema: "BackendChallenge");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsEntity_OrderEntityOrderId",
                schema: "BackendChallenge",
                table: "Product",
                newName: "IX_Product_OrderEntityOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                schema: "BackendChallenge",
                table: "Product",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Order_OrderEntityOrderId",
                schema: "BackendChallenge",
                table: "Product",
                column: "OrderEntityOrderId",
                principalSchema: "BackendChallenge",
                principalTable: "Order",
                principalColumn: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Order_OrderEntityOrderId",
                schema: "BackendChallenge",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                schema: "BackendChallenge",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "Product",
                schema: "BackendChallenge",
                newName: "ProductsEntity",
                newSchema: "BackendChallenge");

            migrationBuilder.RenameIndex(
                name: "IX_Product_OrderEntityOrderId",
                schema: "BackendChallenge",
                table: "ProductsEntity",
                newName: "IX_ProductsEntity_OrderEntityOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsEntity",
                schema: "BackendChallenge",
                table: "ProductsEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsEntity_Order_OrderEntityOrderId",
                schema: "BackendChallenge",
                table: "ProductsEntity",
                column: "OrderEntityOrderId",
                principalSchema: "BackendChallenge",
                principalTable: "Order",
                principalColumn: "OrderId");
        }
    }
}
