using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace imageuploadandmanagementsystem.Migrations
{
    /// <inheritdoc />
    public partial class modifyimagetable_removeImgkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageKey",
                table: "Images");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageKey",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
