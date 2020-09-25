using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Routing.Consumer
{
    class Program
    {
        private static string _Hostname = "localhost";
        private static string _Username = "guest";
        private static string _Password = "guest";

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

                if (args.Length < 1)
                {
                    Console.Error.WriteLine("Usage: {0} [info] [warning] [error]",
                                            Environment.GetCommandLineArgs()[0]);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                    Environment.ExitCode = 1;
                    return;
                }

                var queueName = channel.QueueDeclare().QueueName;

                foreach (var severity in args)
                {
                    channel.QueueBind(
                        queue: queueName,
                        exchange: "routing-exchange",
                        routingKey: severity,
                        arguments: null
                    );
                }

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) => {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;

                    Console.WriteLine(" [x] Received: {0}: {1}", routingKey, message);
                };

                channel.BasicConsume(
                    queue: queueName,
                    autoAck: true,
                    consumer: consumer
                );


                Console.ReadLine();
            }
        }
    }
}
