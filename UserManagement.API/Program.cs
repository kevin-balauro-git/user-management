using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text.Json.Serialization;
using UserManagement.API.Data;
using UserManagement.API.Entities;
using UserManagement.API.Extensions;
using UserManagement.API.Interface;
using UserManagement.API.Repository;
using UserManagement.API.Services;

Log.Logger = new LoggerConfiguration()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting Web API");

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog((serviceProviders, loggerConfiguration) => loggerConfiguration
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(serviceProviders));

    builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: "CorsPolicy",
                policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
        });

    builder.Services.AddProblemDetails();

    builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
    );
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<UserContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("Database"))
    );

    builder.Services.AddIdentityCore<User>(options =>
        {
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
        })
        .AddRoles<Role>()
        .AddRoleManager<RoleManager<Role>>()
        .AddSignInManager<SignInManager<User>>()
        .AddEntityFrameworkStores<UserContext>();

    builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
            options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
            options.DefaultScheme =
            options.DefaultSignInScheme =
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!))
                };
            });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Admin", p => p.RequireClaim("IsAdmin", "true"));
    });

    builder.Services.AddAutoMapper(typeof(Program).Assembly);

    builder.Services.AddScoped<SeedData>();
    builder.Services.AddScoped<IJwtService, JwtService>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();


    WebApplication app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    await app.UseSeedDb();

    app.UseHttpsRedirection();
    app.UseCors("CorsPolicy");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex.Message, "An unhandled exception occurred during bootstrapping");
}
finally
{
    await Log.CloseAndFlushAsync();
}
