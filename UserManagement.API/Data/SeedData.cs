using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.API.Entities;

namespace UserManagement.API.Data
{
    public class SeedData
    {
        public static async Task InitializeDb(UserManager<User> userManager, RoleManager<Role> roleManager)
{
    if (userManager.Users.Any()) { return; }
    
    var users = new User[]
        {
            new User{

                Name = {FirstName="Antonio", LastName="Luna"},
                UserName = "antonluna",
                Email = "antonluna@email.com",
                Password = "antonLun4uno*",
                PhoneNumber= "09588693556",
                Address =
                {
                    City ="Badoc",
                    StreetName = "Bado",
                    StreetNumber = "77",
                    ZipCode="2904",
                    GeoLocation =
                    {
                        Latitude ="17.9085",
                        Longitude = "120.4934"
                    }
                },
                
            },
            new User{

                Name = {FirstName="Francisco", LastName="Dagohoy"},
                UserName = "fdagohoy",
                Email = "fdagojoy@email.com",
                Password = "daG0nsa#uyu#oy",
                PhoneNumber = "09862345678",
                Address =
                {
                    City ="Inabanga",
                    StreetName = "cambitoon",
                    StreetNumber = "1724",
                    ZipCode="6322",
                    GeoLocation =
                    {
                        Latitude ="10.0314",
                        Longitude = "124.0674"
                    }
                },
               
            },
            new User{

                Name = {FirstName="Mohammad", LastName="Kudarat"},
                UserName = "mkudarat",
                Email = "mkudarat@email.com",
                Password = "M!ndanao1",
                PhoneNumber = "09245679284",
                Address =
                {
                    City ="Cotobato",
                    StreetName = "Timog",
                    StreetNumber = "64",
                    ZipCode="9800",
                    GeoLocation =
                    {
                        Latitude ="6.5069",
                        Longitude = "124.4198"
                    }
                },
               
            },
            new User{

                Name = {FirstName="Sultan", LastName="Kudarat"},
                UserName = "mkudarat1",
                Email = "mkudara1t@email.com",
                Password = "M!ndanao1",
                PhoneNumber = "09245679284",
                Address =
                {
                    City ="Cotobato",
                    StreetName = "Timog",
                    StreetNumber = "64",
                    ZipCode="9800",
                    GeoLocation =
                    {
                        Latitude ="6.5069",
                        Longitude = "124.4198"
                    }
                },
               
            }, 
            new User{

                Name = {FirstName="Lakan", LastName="Kudarat"},
                UserName = "mkudarat21",
                Email = "mkudara1t5@email.com",
                Password = "M!ndanao1",
                PhoneNumber = "09245679284",
                Address =
                {
                    City ="Cotobato",
                    StreetName = "Timog",
                    StreetNumber = "64",
                    ZipCode="9800",
                    GeoLocation =
                    {
                        Latitude ="6.5069",
                        Longitude = "124.4198"
                    }
                },
                
            }
            
        };
    var roles = new List<Role>
    {
        new Role{ Name="Employee"},
        new Role{ Name="Admin"},
        new Role{ Name="Moderator"}
    };

    var admins = new User[] { 
        
    new User() {UserName = "admin",Email = "admin@email.com", Password = "Pa$$w0rd" },
   
    new User() {

        Name = { FirstName = "Juan", LastName = "Luna" },
        UserName = "juanluna",
        Email = "juanluna@email.com",
        Password = "juanLun4uno*",

        Address =
                {
                    City ="Badoc",
                    StreetName = "Bado",
                    StreetNumber = "77",
                    ZipCode="2904",
                    GeoLocation =
                    {
                        Latitude ="17.9085",
                        Longitude = "120.4934"
                    }
                },
        PhoneNumber = "09583452456",
        }
    };
    
    

    foreach (Role role in roles)
    {
        await roleManager.CreateAsync(role);
    }

    foreach (User user in users)
    {  
        await userManager.CreateAsync(user, user.Password);
        await userManager.AddToRoleAsync(user, "Employee");
    }

    foreach (User admin in admins)
    {
        await userManager.CreateAsync(admin, admin.Password);
        await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
    }

    
}
    }
}
