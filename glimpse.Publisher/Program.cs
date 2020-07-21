using glimpse.Models;
using glimpse.Models.Messaging;
using glimpse.Models.Repository;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace glimpse.Publisher
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var config = builder.Build();

            var connectionString = config["Rabbit"];

            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(connectionString)
            };

            var connection = new RabbitPersistentConnection(connectionFactory);
            var publisher = new HttpEventPublisher(connection, HttpEventManager.TestData());

            while (true)
            {
                Console.WriteLine("Press CTRL+C to quit.");

                var text = Console.ReadLine();

                Console.WriteLine("message sent!");
            }
        }
    }
}