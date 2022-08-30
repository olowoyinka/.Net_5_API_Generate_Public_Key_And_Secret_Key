using Microsoft.EntityFrameworkCore.Migrations;

namespace EncryptionExample.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "Foods",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "Foods");
        }
    }
}
