using Microsoft.AspNetCore.Identity;
using UserManagement.API.Data;
using UserManagement.API.Entities;

namespace UserManagement.API.Extensions
{
    public static class SeedExtension 
    {
        public static async Task<IApplicationBuilder> UseSeedDb(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<UserContext>();
                var i = services.GetRequiredService<UserManager<AccessUser>>();
               await SeedData.InitializeDb(context,i);
            }
            catch (Exception ex)
            {

            }
            return app;
        }
    }
}
