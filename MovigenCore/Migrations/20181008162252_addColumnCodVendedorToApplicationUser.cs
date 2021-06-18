using Microsoft.EntityFrameworkCore.Migrations;

namespace MovigenCore.Migrations
{
    public partial class addColumnCodVendedorToApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodVendedor",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodVendedor",
                table: "AspNetUsers");
        }
    }
}
