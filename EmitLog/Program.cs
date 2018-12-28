using System;
using System.Text;
using RabbitMQ.Client;

namespace EmitLog
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory {HostName = "localhsot"};
            using (var connetion = factory.CreateConnection())
            {
                using (var channel = connetion.CreateModel())
                {
                    //声明一个名称为"logs"的exchange,并且指定exchange的类型为fanout
                    channel.ExchangeDeclare(exchange:"logs",type:"fanout");
                    string message = args.Length > 0 ? string.Join(" ", args) : "Publish or Subscribe demo";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(
                        exchange:"logs",
                        routingKey:"",
                        basicProperties:null,
                        body:body);
                    Console.WriteLine($"Tht send message:{message}");
                }
                Console.WriteLine("Press Enter to Exit");
                Console.ReadLine();
            }
        }
    }
}
