using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDbClient.Caching.Infrastructure;

namespace MongoDbClient.ConsoleApp
{
    public class Startup
    {
        public Startup()
        {
            Configuration = GetConfiguration();
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            services.AddSingleton(Configuration);

            services.AddCaching();

            services.Configure<Settings>(options =>
            {
                options.ConnectionString
                    = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database
                    = Configuration.GetSection("MongoConnection:Database").Value;
            });

            services.AddTransient<ICustomerRepository, CustomerRepository>();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
        }

        internal static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true);

            return builder.Build();
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(GetConfiguration());

            var serviceProvider = services.BuildServiceProvider();
            //serviceProvider.ConfigureLoggerFactory();

            var logger = serviceProvider.GetService<ILogger<Startup>>();

            var exception = (Exception) unhandledExceptionEventArgs.ExceptionObject;

            if (exception is AggregateException)
            {
                exception = ((AggregateException) exception).Flatten();
            }

            logger.LogError(exception, exception.Message);
        }
    }
}