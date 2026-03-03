﻿using Npgsql;      
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MLFamilyTravelBlog.Models;

namespace MLFamilyTravelBlog.Data
{
    public static class DataUtility
    {

        // Admin & Moderator - use with roles
        private const string? _adminRole = "Admin";
        private const string? _moderatorRole = "Moderator";

        public static string GetConnectionString(IConfiguration configuration)
        {
            // Use this method to get the connection string for your database
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString! : BuildConnectionString(databaseUrl);
        }

        private static string BuildConnectionString(string databaseUrl)
        {
            // Implement your logic to build the connection string from the database URL
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            var builder = new Npgsql.NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.AbsolutePath.TrimStart('/'),
                SslMode = Npgsql.SslMode.Prefer
            };
            return builder.ConnectionString;
        }

        public static async Task ManageDataAsync(IServiceProvider serviceProvider)
        {
            var dbContextSvc = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManagerSvc = serviceProvider.GetRequiredService<UserManager<BlogUser>>();
            var configurationSvc = serviceProvider.GetRequiredService<IConfiguration>();
            var roleManagerSvc = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // align the database by checking the migrations
            await dbContextSvc.Database.MigrateAsync();

            // Seed some info
            await SeedRolesAsync(roleManagerSvc);
            await SeedBlogUsersAsync(userManagerSvc, configurationSvc);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(_adminRole!))
            {
                await roleManager.CreateAsync(new IdentityRole(_adminRole!));
            }

            if (!await roleManager.RoleExistsAsync(_moderatorRole!))
            {
                await roleManager.CreateAsync(new IdentityRole(_moderatorRole!));
            }
        }

        private static async Task SeedBlogUsersAsync(UserManager<BlogUser> userManager, IConfiguration configuration)
        {
            string? adminEmail = configuration["AdminEmail"] ?? Environment.GetEnvironmentVariable("AdminEmail");
            string? adminPassword = configuration["AdminPWD"] ?? Environment.GetEnvironmentVariable("AdminPWD");
            string? moderatorEmail = configuration["ModeratorEmail"] ?? Environment.GetEnvironmentVariable("ModeratorEmail");
            string? moderatorPassword = configuration["ModeratorPWD"] ?? Environment.GetEnvironmentVariable("ModeratorPWD");

            // Validate required configuration values
            if (string.IsNullOrWhiteSpace(adminEmail))
            {
                throw new InvalidOperationException("AdminEmail is not configured. Please add it to User Secrets or environment variables.");
            }
            if (string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new InvalidOperationException("AdminPWD is not configured. Please add it to User Secrets or environment variables.");
            }
            if (string.IsNullOrWhiteSpace(moderatorEmail))
            {
                throw new InvalidOperationException("ModeratorEmail is not configured. Please add it to User Secrets or environment variables.");
            }
            if (string.IsNullOrWhiteSpace(moderatorPassword))
            {
                throw new InvalidOperationException("ModeratorPWD is not configured. Please add it to User Secrets or environment variables.");
            }

            try
            {
                BlogUser? adminUser = new()
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Jerry",
                    LastName = "McKee",
                    EmailConfirmed = true
                };


                BlogUser? blogUser = await userManager.FindByEmailAsync(adminEmail);


                if (blogUser == null)
                {
                    IdentityResult createResult = await userManager.CreateAsync(adminUser, adminPassword);
                    if (createResult.Succeeded)
                    {
                        // Refetch the user from database to ensure we have the persisted entity
                        adminUser = await userManager.FindByEmailAsync(adminEmail);
                        if (adminUser != null)
                        {
                            await userManager.AddToRoleAsync(adminUser, _adminRole!);
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Failed to create admin user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                        Console.ResetColor();
                    }
                }


                BlogUser? moderatorUser = new()
                {
                    UserName = moderatorEmail,
                    Email = moderatorEmail,
                    FirstName = "John",
                    LastName = "Smith",
                    EmailConfirmed = true
                };

                blogUser = await userManager.FindByEmailAsync(moderatorEmail);

                if (blogUser == null)
                {
                    IdentityResult createResult = await userManager.CreateAsync(moderatorUser, moderatorPassword);
                    if (createResult.Succeeded)
                    {
                        // Refetch the user from database to ensure we have the persisted entity
                        moderatorUser = await userManager.FindByEmailAsync(moderatorEmail);
                        if (moderatorUser != null)
                        {
                            await userManager.AddToRoleAsync(moderatorUser, _moderatorRole!);
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Failed to create moderator user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                        Console.ResetColor();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("****************** ERROR *****************");
                Console.WriteLine($"Failure Seeding Default Blog Users Error: {ex.Message}");
                Console.WriteLine("****************** ERROR *****************");
                Console.ResetColor();
                throw;
            }

        }
    }
}