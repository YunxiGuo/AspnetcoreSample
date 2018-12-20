using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMQNewTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            //创建连接
            using (var connection = factory.CreateConnection())
            {
                //创建信道
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "taskwork", //队列名
                        durable: true,  //是否持久化,防止队列和消息丢失
                        exclusive: false,   //是否排外,
                        autoDelete: false,  //是否自动删除队列
                        arguments: null);
                    var message = GetMessage(args);
                    var body = Encoding.UTF8.GetBytes(message);
                    //var 
                }
            }
        }

        private static string GetMessage(string[] arg)
        {
            return (arg.Length > 0 ? string.Join("", arg) : "Hello world!");
        }
    }
}
