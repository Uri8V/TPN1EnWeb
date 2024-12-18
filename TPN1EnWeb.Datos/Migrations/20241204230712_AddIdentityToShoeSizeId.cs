using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPN1EnWeb.Datos.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityToShoeSizeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Crear una columna temporal para almacenar los datos
            migrationBuilder.AddColumn<int>(
                name: "TempShoeSizeId",
                table: "ShoeSizes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Copiar los datos de la columna original (ShoeSizeId) a la nueva columna temporal
            migrationBuilder.Sql("UPDATE ShoeSizes SET TempShoeSizeId = ShoeSizeId");

            // Eliminar la columna original ShoeSizeId
            migrationBuilder.DropColumn(name: "ShoeSizeId", table: "ShoeSizes");

            // Crear la nueva columna ShoeSizeId con la propiedad IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "ShoeSizeId",
                table: "ShoeSizes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Copiar los datos de la columna temporal a la nueva columna con IDENTITY
            // NOTA: Esto puede causar problemas si tienes datos con valores repetidos
            migrationBuilder.Sql("SET IDENTITY_INSERT ShoeSizes ON; " +
                                 "INSERT INTO ShoeSizes (ShoeSizeId) SELECT TempShoeSizeId FROM ShoeSizes; " +
                                 "SET IDENTITY_INSERT ShoeSizes OFF;");

            // Eliminar la columna temporal
            migrationBuilder.DropColumn(name: "TempShoeSizeId", table: "ShoeSizes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revertir los cambios si es necesario
            migrationBuilder.DropColumn(name: "ShoeSizeId", table: "ShoeSizes");
            migrationBuilder.AddColumn<int>(
                name: "ShoeSizeId",
                table: "ShoeSizes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
