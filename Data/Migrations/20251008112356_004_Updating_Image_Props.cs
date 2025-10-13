using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLFamilyTravelBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class _004_Updating_Image_Props : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFile",
                table: "BlogPosts");

            migrationBuilder.AlterColumn<string>(
                name: "ImageType",
                table: "BlogPosts",
                type: "text",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "bytea",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "BlogPosts",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "BlogPosts");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ImageType",
                table: "BlogPosts",
                type: "bytea",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFile",
                table: "BlogPosts",
                type: "text",
                nullable: true);
        }
    }
}
