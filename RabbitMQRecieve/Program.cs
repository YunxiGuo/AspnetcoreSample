using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQRecieve
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
                    channel.QueueDeclare(
                        queue: "hello",
                        durable: false,
                        exclusive: false, 
                        autoDelete: false,
                        arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += Consumer_Received;
                    channel.BasicConsume(queue: "hello",
                        autoAck: true,
                        consumer: consumer);
                    Console.WriteLine("Press [enter] to exit");

                    Console.ReadLine();
                }
            }
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var msg = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[x] Recieve {msg}");
        }
    }
}
