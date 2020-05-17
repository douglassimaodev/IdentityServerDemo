using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityServerDemo.BlazorClient.Helpers;

namespace IdentityServerDemo.BlazorClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");


            //I know that there is a way to configure here so httpclient will add token in the request, but for me it didnt work
            //Eu seu que tem uma forma de configurar aqui para o httpclient add o token de autenticacao nos request, mas nao funcionou para mim

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44300") });
            //https://localhost:44300 is my webapi 

            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // Values are in the wwwroot/appsettings.json
                builder.Configuration.Bind("Local", options.ProviderOptions);
            });

            builder.Services.AddOptions();
            builder.Services.AddScoped<IHttpService, HttpService>();
            builder.Services.AddTransient<CustomHttpClientFactory>();
            //services.AddFileReaderService(options => options.InitializeOnFirstCall = true);            

            await builder.Build().RunAsync();
        }
    }
}
