using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPN1EnWeb.Datos.Migrations
{
    /// <inheritdoc />
    public partial class AddingListOrderHeaderInApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ShoeSizes_ShoeSizesShoeSizeId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ShoeSizesShoeSizeId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ShoeSizesShoeSizeId",
                table: "OrderDetails");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ShoeSizeId",
                table: "OrderDetails",
                column: "ShoeSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ShoeSizes_ShoeSizeId",
                table: "OrderDetails",
                column: "ShoeSizeId",
                principalTable: "ShoeSizes",
                principalColumn: "ShoeSizeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ShoeSizes_ShoeSizeId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ShoeSizeId",
                table: "OrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ShoeSizes_ShoeSizesShoeSizeId",
                table: "OrderDetails",
                column: "ShoeSizesShoeSizeId",
                principalTable: "ShoeSizes",
                principalColumn: "ShoeSizeId");
        }
    }
}
