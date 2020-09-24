using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Publish.Consumer
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

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(
                    queue: queueName, 
                    exchange: "publish-exchange", 
                    routingKey: "", 
                    arguments: null
                );

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) => {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine(" [x] Received: {0}", message);
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
