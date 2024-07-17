using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UserManagement.API.Data;
using UserManagement.API.Entities;

namespace UserManagement.API.Extensions
{
    public static class SeedExtension
    {
        public static async Task<IApplicationBuilder> UseSeedDb(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userContext = scope.ServiceProvider.GetRequiredService<UserContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

                await userContext.Database.MigrateAsync();
                await SeedData.InitializeDb(userManager, roleManager);

            }

            return app;
        }
    }
}
