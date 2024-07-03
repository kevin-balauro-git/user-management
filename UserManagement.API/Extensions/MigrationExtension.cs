using Microsoft.EntityFrameworkCore;
using UserManagement.API.Entities;

namespace UserManagement.API.Extensions
{
    public static class MigrationExtension
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using UserContext userContext = scope.ServiceProvider.GetRequiredService<UserContext>();

            userContext.Database.Migrate();
        }

    }
}
