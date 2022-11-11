// From LosFusion.CarStore.DataAccessLayer
// Add-Migration InitialCreation
// Update-Database

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc.Authorization;
//using Microsoft.Identity.Web;
using Okta.AspNetCore;
using LosFusion.CarStore.DataAccessLayer;
using LosFusion.CarStore.DataAccessLayer.Repositories;
using LosFusion.CarStore.BusinessLogicLayer.Interfaces;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;

namespace LosFusion.CarStore.ServiceLayer;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
        //    var builder = WebApplication.CreateBuilder(args);

        //    // Add services to the container.

        //    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        //    builder.Services.AddDbContext<CarDbContext>(options => options.UseSqlServer(connectionString));

        //    builder.Services
        //        .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        //        .AddEntityFrameworkStores<CarDbContext>();

        //    //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
        //    builder.Services
        //        .AddAuthentication(options => 
        //        {
        //            options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
        //            options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
        //            options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
        //        })
        //        .AddOktaWebApi(new OktaWebApiOptions()
        //         {
        //             OktaDomain = builder.Configuration["Okta:OktaDomain"],
        //             AuthorizationServerId = builder.Configuration["Okta:AuthorizationServerId"],
        //             Audience = builder.Configuration["Okta:Audience"]
        //         });

        //    builder.Services.AddAuthorization();
        //    builder.Services.AddControllers().AddNewtonsoftJson();
        //    builder.Services.AddScoped<ICarRepository, CarRepository>();

        //    builder.Services.AddEndpointsApiExplorer();
        //    builder.Services.AddSwaggerGen();

        //    var app = builder.Build();

        //    if (app.Environment.IsDevelopment())
        //    {
        //        app.UseSwagger();
        //        app.UseSwaggerUI();
        //    }

        //    app.UseHttpsRedirection();
        //    app.UseAuthentication();
        //    app.UseAuthorization();

        //    app.MapControllers();

        //    app.Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}