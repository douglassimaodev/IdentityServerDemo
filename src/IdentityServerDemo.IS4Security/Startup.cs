using IdentityServerDemo.IS4Security.Data;
using IdentityServerDemo.IS4Security.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServerDemo.IS4Security
{
    //I use dotnet new is4aspid template to create it
    //and then removed ef sqlit and installed ef sql server

    //Eu usei dotnet new is4aspid para criar esse projeto
    //Depois eu removi o Ef Sqlite e instalei o EF sql server

    //After that delete migration and add-migration dbInit again to create it with sql server
    //Depois delete a pasta migrations dentro de Data e crie ela de novo add-migration dbInit para usar o sql server

    //after add Identity for registration and other things that you want,
    //right click in the project -> add -> new scaffolded item -> Identity and select the items you want

    //Default User: demo@demo.com pass: @demo123456
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            //For Identity
            services.AddRazorPages();

            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //here you can configure the way you want to use Identity (it is not Identity server)
                //Aqui tu pode configurar o teu Identity (não é Identity server)
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = false;

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
                {
                    //Here you can change the route in case you want to custom your url
                    //Aqui tu pode trocar a rota em caso tu queiras alterar e colocar em outro caminho

                    //options.UserInteraction.LoginUrl = "/Account/Login";
                    //options.UserInteraction.LogoutUrl = "/Account/Logout";

                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<ApplicationUser>();

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to http://localhost:5000/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                //For Identity
                endpoints.MapRazorPages();
            });
        }
    }
}