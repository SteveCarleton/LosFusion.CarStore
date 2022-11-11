//using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc.Authorization;
//using Microsoft.Identity.Web;
using Okta.AspNetCore;
using LosFusion.CarStore.DataAccessLayer;
using LosFusion.CarStore.DataAccessLayer.Repositories;
using LosFusion.CarStore.BusinessLogicLayer.Interfaces;

namespace LosFusion.CarStore.ServiceLayer;

public class Startup
{
    public IConfiguration Configuration { get; }
    readonly IWebHostEnvironment webHostingEnvironment;

    public Startup(IConfiguration configuration, IWebHostEnvironment webHostingEnvironment)
    {
        Configuration = configuration;
        this.webHostingEnvironment = webHostingEnvironment;
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<CarDbContext>(options => options.UseSqlServer(connectionString));

        services
            .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<CarDbContext>();

        //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
                options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
                options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
            })
            .AddOktaWebApi(new OktaWebApiOptions()
            {
                OktaDomain = Configuration["Okta:OktaDomain"],
                AuthorizationServerId = Configuration["Okta:AuthorizationServerId"],
                Audience = Configuration["Okta:Audience"]
            });

        services.AddAuthorization();
        services.AddControllers().AddNewtonsoftJson();
        services.AddScoped<ICarRepository, CarRepository>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
