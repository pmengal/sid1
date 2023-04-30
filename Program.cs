using Microsoft.Extensions.Configuration;
using Serilog;
using Spectre.Console.Cli;
using SID1.Commands;
using Spectre.Console;

namespace SID1
{
    internal class Program
    {
        public static async Task<int> Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false)
                            .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                var app = new CommandApp();
                app.Configure(config =>
                {
                    config.AddCommand<AzureCommand>("azure");
                    //config.AddCommand<GoogleCommand>("google"); // For future speech services
                });

                return await app.RunAsync(args);
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                return -99;
            }
        }
    }
}