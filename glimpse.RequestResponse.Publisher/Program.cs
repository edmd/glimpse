using glimpse.Data;
using glimpse.Models;
using glimpse.Models.Queue;
using glimpse.Models.Repository;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace glimpse.RequestResponses.Publisher
{
    public class Program
    {
        static void Main(string[] args)
        {
            //var builder = new ConfigurationBuilder();
            //builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            //var config = builder.Build();

            //var connectionString = config["Rabbit"];

            //var connectionFactory = new ConnectionFactory()
            //{
            //    Uri = new Uri(connectionString)
            //};

            //// Instantiate the EventManager
            //var connection = new RabbitPersistentConnection(connectionFactory);
            //var repository = new RequestResponseRepository(new DataContext());
            //var eventManager = new HttpEventManager(repository, connection);

            //while (true)
            //{
            //    Console.WriteLine("Press CTRL+C to quit.");

            //    var text = Console.ReadLine();

            //    Console.WriteLine("message sent!");
            //}
        }
    }
}