using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MongoDbClient.Caching;
using MongoDbClient.ConsoleApp.Models;

namespace MongoDbClient.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            var startup = new Startup();
            startup.ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();

            using (var stream = File.OpenRead(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "input", "MOCK_DATA.csv")))
            {
                var csvLines = new CsvDataReader().Read(stream);

                Console.WriteLine($"{csvLines.Count()}");
            }


            //var repo = serviceProvider.GetService<ICustomerRepository>();

                //var index = serviceProvider.GetService<ICustomerIndexEngine>();

                //Console.WriteLine("Hello World!");

                //var customers = repo.GetCustomersById(new[] {"ID"}).Result;
                //Console.WriteLine($"Notes count: {customers.Count()}");

                //foreach (var customer in customers)
                //{
                //    Console.WriteLine($"{customer.FirstName}, {customer.Surname}");
                //}

                //var newCustomer = new Customer
                //{
                //    Id = "ID",
                //    FirstName = "FirstName",
                //    Surname = "Surname",
                //    Address = "Address",
                //    EmailAddress = "EmailAddress",
                //    PhoneNumber = "PhoneNumber",
                //    UpdatedOn = DateTime.Now
                //};

                //repo.AddOrUpdateCustomer("ID", newCustomer);

                //index.AddOrUpdateCustomer(newCustomer.Id, newCustomer.FirstName, newCustomer.)

                Console.ReadLine();
        }
    }
}