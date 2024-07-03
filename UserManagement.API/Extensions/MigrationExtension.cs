using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using UserManagement.API.Entities;

namespace UserManagement.API.Extensions
{
    public static  class MigrationExtension
    {
        public static void UseMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using UserContext userContext = scope.ServiceProvider.GetService<UserContext>();

            userContext.Database.Migrate();
        }
    }
}
