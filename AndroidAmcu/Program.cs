using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AndroidAmcu
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()

                   .UseKestrel()
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .UseIISIntegration()
                   .UseStartup<Startup>()
                   .Build();
            host.Run();
        }

        public static IWebHost CreateWebHostBuilder(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
              .UseStartup<Startup>().Build();
    }
}
