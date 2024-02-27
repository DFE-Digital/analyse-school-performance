using Microsoft.Extensions.Hosting;

namespace ASP.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new HostBuilder();
            new Startup().Configure(builder);
            var host = builder.Build();

            host.Run();
        }
    }
}