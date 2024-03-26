// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using TestConsumer;

internal class Program
{
    private static void Main(string[] args)
    {
        Startup.CreateHostBuilder(args).Build().Run();
    }
}