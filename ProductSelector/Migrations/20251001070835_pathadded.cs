using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductSelector.Migrations
{
    /// <inheritdoc />
    public partial class pathadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectPath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectPath",
                table: "Products");
        }
    }
}
