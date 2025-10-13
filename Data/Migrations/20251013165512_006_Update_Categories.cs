using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLFamilyTravelBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class _006_Update_Categories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFile",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "ImageType",
                table: "Categories",
                type: "text",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Categories",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "BlogPosts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "BlogPosts");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ImageType",
                table: "Categories",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFile",
                table: "Categories",
                type: "text",
                nullable: true);
        }
    }
}
