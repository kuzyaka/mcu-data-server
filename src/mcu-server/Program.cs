using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace McuServerApi
{
    public static class McuApiProgram
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(b =>
                {
                    b.ConfigureKestrel(opt => { opt.Limits.MinResponseDataRate = null; });
                    b.UseKestrel();
                    b.UseStartup<Startup>();
                });
    }
}