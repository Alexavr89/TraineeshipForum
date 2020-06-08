using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(TraineeshipForum.Areas.Identity.IdentityHostingStartup))]
namespace TraineeshipForum.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}