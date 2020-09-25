using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace Routing.Producer
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
                    exchange: "routing-exchange",
                    type: ExchangeType.Direct
                );

                var severity = (args.Length > 0) ? args[0] : "info";

                var message = (args.Length > 1)
                                ? string.Join(' ', args.Skip(1).ToArray())
                                : "Hello world!!!";

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: "routing-exchange",
                    routingKey: severity,
                    basicProperties: null,
                    body: body
                );
            }
        }
    }
}
