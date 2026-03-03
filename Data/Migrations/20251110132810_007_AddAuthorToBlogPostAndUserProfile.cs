using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLFamilyTravelBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class _007_AddAuthorToBlogPostAndUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add user profile columns first
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "AspNetUsers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GitHubUrl",
                table: "AspNetUsers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramUrl",
                table: "AspNetUsers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "AspNetUsers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "AspNetUsers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwitterUrl",
                table: "AspNetUsers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            // Add AuthorId column as nullable first to allow existing records
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "BlogPosts",
                type: "text",
                nullable: true);

            // Get or create a default system user for existing blog posts
            migrationBuilder.Sql(@"
                DO $$
                DECLARE
                    default_user_id text;
                BEGIN
                    -- Try to get the first existing user
                    SELECT ""Id"" INTO default_user_id 
                    FROM ""AspNetUsers"" 
                    LIMIT 1;

                    -- If no users exist, create a system user
                    IF default_user_id IS NULL THEN
                        default_user_id := gen_random_uuid()::text;
                        INSERT INTO ""AspNetUsers"" (
                            ""Id"", 
                            ""UserName"", 
                            ""NormalizedUserName"", 
                            ""Email"", 
                            ""NormalizedEmail"", 
                            ""EmailConfirmed"", 
                            ""PasswordHash"", 
                            ""SecurityStamp"", 
                            ""ConcurrencyStamp"",
                            ""PhoneNumberConfirmed"",
                            ""TwoFactorEnabled"",
                            ""LockoutEnabled"",
                            ""AccessFailedCount"",
                            ""FirstName"",
                            ""LastName""
                        )
                        VALUES (
                            default_user_id,
                            'system@blog.local',
                            'SYSTEM@BLOG.LOCAL',
                            'system@blog.local',
                            'SYSTEM@BLOG.LOCAL',
                            true,
                            'AQAAAAIAAYagAAAAEJVx4N8VwGC7x3JqLH0JR8GqTHbQjJlQI7vKQK8YtBqkB7L8xZ5Lx0LJ3L8xZ5Lx==',
                            gen_random_uuid()::text,
                            gen_random_uuid()::text,
                            false,
                            false,
                            true,
                            0,
                            'System',
                            'User'
                        );
                    END IF;

                    -- Update all existing BlogPosts to use this user
                    UPDATE ""BlogPosts""
                    SET ""AuthorId"" = default_user_id
                    WHERE ""AuthorId"" IS NULL;
                END $$;
            ");

            // Now make AuthorId required
            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "BlogPosts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_AuthorId",
                table: "BlogPosts",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_AspNetUsers_AuthorId",
                table: "BlogPosts",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_AspNetUsers_AuthorId",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_AuthorId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GitHubUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InstagramUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwitterUrl",
                table: "AspNetUsers");
        }
    }
}
