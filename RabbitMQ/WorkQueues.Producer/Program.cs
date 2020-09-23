using RabbitMQ.Client;
using System;
using System.Text;

namespace WorkQueues.Producer
{
    class Program
    {
        private const string _HostName = "localhost";
        private const string _Username = "guest";
        private const string _Password = "guest";

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _HostName,
                UserName = _Username,
                Password = _Password
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "work-queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "work-queue",
                    basicProperties: properties,
                    body: body
                );
            }
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join("", args) : "Hello World!!!");
        }
    }
}
