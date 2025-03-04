using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTest
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
          
            builder.ConfigureServices(services =>
            {
                // remove real connection and use SQLite in memory.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IDbConnectionFactory));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddSingleton<IDbConnectionFactory>(new TestDbConnectionFactory());
            });
        }

       
    }
}
