using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Consul;

namespace ConsulClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var consul = new Consul.ConsulClient(c =>
            {
                c.Address = new Uri("http://127.0.0.1:8500");
            }))
            {
                //取出全部的MsgService服务
                var services = consul.Agent.Services().Result.Response.Values.Where(p => p.Service.Equals("WebApi1Service", StringComparison.OrdinalIgnoreCase));
                var agentServices = services as AgentService[] ?? services.ToArray();
                if (!agentServices.Any())
                {
                    Console.WriteLine($"名称为【WebApi1Service】的服务均为上线");
                    return;
                }
                //客户端负载均衡，随机选出一台服务
                Random rand = new Random();
                var index = rand.Next(agentServices.Count());
                var s = agentServices.ElementAt(index);
                Console.WriteLine($"Index={index},ID={s.ID},Service={s.Service},Addr={s.Address},Port={s.Port}");
                //向服务发送请求
                var httpClient = new HttpClient();
                var httpContent =
                    new StringContent("{phoneNum:'119',msg:'help me'}", Encoding.UTF8, "application/json");
                var url = $"http://{s.Address}:{s.Port}/api1/Sms/Send_LX";//地址
                var result = httpClient.PostAsync(url, httpContent).GetAwaiter().GetResult();
                Console.WriteLine($"调用{s.Service}，状态：{result.StatusCode}");
            }
            
            Console.ReadKey();
        }
    }
}
