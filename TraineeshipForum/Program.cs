using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TraineeshipForum
{
    public class Program
    {
            public static void Main(string[] args)
            {
                BuildWebHost(args).Run();
            }

             public static IWebHost BuildWebHost(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((builderContext, config) =>
                {
                    IWebHostEnvironment env = builderContext.HostingEnvironment;
                    config
                        .AddJsonFile("storageSettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                })
                .UseStartup<Startup>()
                .Build();
    }
}
