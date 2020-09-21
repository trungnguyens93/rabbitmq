using RabbitMQ.Client;
using System;
using System.Text;

namespace Basic.Producer
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
                    queue: "basic-queue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(
                    exchange: "",
                    routingKey: "basic-queue",
                    basicProperties: null,
                    body: body
                );

                Console.WriteLine(" [X] Sent: {0}", message);
            }

            Console.WriteLine(" Press [Enter] to exit!!!");
            Console.ReadLine();
        }
    }
}
