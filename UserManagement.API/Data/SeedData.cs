using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.API.Entities;

namespace UserManagement.API.Data
{
    public class SeedData
    {
        public static async Task InitializeDb(UserContext context, UserManager<AccessUser> accessManager)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            
            context.Database.EnsureDeleted();
            context.Database.Migrate();
            context.Database.EnsureCreated();

            var users = new User[]
                {
                    new User{

                        Name = {FirstName="Juan", LastName="Luna"},
                        Username = "juanluna",
                        Email = "juanluna@email.com",
                        Password = "juanLun4uno*",
                        Phone = "09583452456",
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
                        IsAdmin = "true",
                    },
                    new User{

                        Name = {FirstName="Antonio", LastName="Luna"},
                        Username = "antonluna",
                        Email = "antonluna@email.com",
                        Password = "juanLun4uno*",
                        Phone = "09583452456",
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
                        IsAdmin = "true",
                    },
                    new User{

                        Name = {FirstName="Francisco", LastName="Dagohoy"},
                        Username = "fdagohoy",
                        Email = "fdagojoy@email.com",
                        Password = "daG0nsa#uyu#oy",
                        Phone = "09862345678",
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
                        IsAdmin = "false",
                    },
                    new User{

                        Name = {FirstName="Mohammad", LastName="Kudarat"},
                        Username = "mkudarat",
                        Email = "mkudarat@email.com",
                        Password = "M!ndanao1",
                        Phone = "09245679284",
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
                        IsAdmin = "false"
                    },
                     new User{

                        Name = {FirstName="Sultan", LastName="Kudarat"},
                        Username = "mkudarat1",
                        Email = "mkudara1t@email.com",
                        Password = "M!ndanao1",
                        Phone = "09245679284",
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
                        IsAdmin = "false"
                    }, new User{

                        Name = {FirstName="Lakan", LastName="Kudarat"},
                        Username = "mkudarat21",
                        Email = "mkudara1t5@email.com",
                        Password = "M!ndanao1",
                        Phone = "09245679284",
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
                        IsAdmin = "false"
                    }

                };

            var accessUsers = new AccessUser[users.Length];
            for (int i = 0; i < users.Length; i++)
            {

                accessUsers[i] = new AccessUser
                {
                    Id = users[i].Uuid.ToString(),
                    UserName = users[i].Username,
                    Email = users[i].Email,
                    IsAdmin = users[i].IsAdmin,
                };
                
            }

            for (int i = 0; i < accessUsers.Length; i++)
            {
                await accessManager.CreateAsync(accessUsers[i], users[i].Password);      
            }


            foreach (User user in users)
            {
                 context.Users.Add(user);
            }
            
            await context.SaveChangesAsync();
            
        }
    }
}
