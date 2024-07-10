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
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userContext = scope.ServiceProvider.GetRequiredService<UserContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AccessUser>>();

                await userContext.Database.MigrateAsync();
                if (!userContext.Users.Any())
                {
                    await SeedData.InitializeDb(userContext, userManager);
                }
            }
            
            return app;
        }
    }
}
