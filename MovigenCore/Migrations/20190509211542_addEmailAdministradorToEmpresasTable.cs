using Microsoft.EntityFrameworkCore.Migrations;

namespace MovigenCore.Migrations
{
    public partial class addEmailAdministradorToEmpresasTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAdministrador",
                table: "Empresa",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAdministrador",
                table: "Empresa");
        }
    }
}
