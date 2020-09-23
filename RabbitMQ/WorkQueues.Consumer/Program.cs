﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace WorkQueues.Consumer
{
    class Program
    {
        private const string _HostName = "localhost";
        private const string _Username = "guest";
        private const string _Password = "guest";

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() {
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

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine(" [x] Received: {0}", message);

                    int dots = message.Split('.').Length - 1;
                    Thread.Sleep(dots * 1000);

                    Console.WriteLine(" [x] Done!!!");

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                channel.BasicConsume(
                    queue: "work-queue",
                    autoAck: true,
                    consumer: consumer
                );
                
                Console.ReadLine();
            }
        }
    }
}
