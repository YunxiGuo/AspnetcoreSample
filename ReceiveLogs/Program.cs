using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ReceiveLogs
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
                    channel.ExchangeDeclare(exchange:"logs",type:"fanout");
                    var queueName = channel.QueueDeclare().QueueName;
                    Console.WriteLine($"{nameof(queueName)}:{queueName}");
                    channel.QueueBind(queue:queueName,exchange:"logs",routingKey:"");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += Consumer_Received;

                    channel.BasicConsume(queueName, autoAck: true, consumer: consumer);
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Receive Message:{message}");
        }
    }
}
