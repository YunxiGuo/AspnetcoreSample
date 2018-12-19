using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQSend
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
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
                    string msg = "Hello RabbitMQ";
                    var body = Encoding.UTF8.GetBytes(msg);
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "hello",
                        mandatory: false,
                        basicProperties: null,
                        body: body);
                    Console.WriteLine($"[x] Send {msg}");
                }
            }
            Console.WriteLine("Press [enter] to exit");

            Console.ReadLine();
        }
    }
}
