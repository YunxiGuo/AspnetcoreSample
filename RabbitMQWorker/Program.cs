using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() {HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //声明一个队列
                    channel.QueueDeclare(
                        queue: "taskwork",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    channel.BasicQos(prefetchSize:0,prefetchCount:1,global:false);

                    Console.WriteLine(" [*] Waiting for messages.");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"[x] Received {message}");

                        int dots = message.Split('.').Length - 1;
                        Thread.Sleep(dots * 1000);

                        Console.WriteLine($"[x] done");

                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };

                    channel.BasicConsume(queue: "taskwork",
                        autoAck: false,
                        consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
