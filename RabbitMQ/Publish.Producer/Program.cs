using RabbitMQ.Client;
using System;
using System.Text;

namespace Publish.Producer
{
    class Program
    {
        private const string _Hostname = "localhost";
        private const string _Username = "guest";
        private const string _Password = "guest";

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() 
            { 
                HostName = _Hostname,
                UserName = _Username,
                Password = _Password
            };

            using (var connection = factory.CreateConnection()) 
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(
                    exchange: "publish-exchange",
                    type: ExchangeType.Fanout
                );

                string message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(
                    exchange: "publish-exchange",
                    routingKey: "",
                    basicProperties: null,
                    body: body
                );

                Console.WriteLine(" [x] Sent: {0}", message);
            }
        }

        private static string GetMessage(string[] args)
        {
            return args.Length > 0 ? string.Join(' ', args) : "Hello World!!!";
        }
    }
}
