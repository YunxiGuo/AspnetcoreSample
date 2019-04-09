using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace WebApi1
{
    public class Startup
    {
        private const string webApi1Id = "EEE93161-D274-4F5B-84E3-885AFAB1F4CF";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WebApi1",
                    Description = "Test SwaggerGen",
                    Version = "v1.0",
                    Contact = new OpenApiContact
                    {
                        Name = "GuoYunxi",
                        Email = "guoyunxi1198@163.com",
                        Url = new Uri("http://www.cnblogs.com/guoyunxi/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "许可证",
                        Url = new Uri("http://www.cnblogs.com/guoyunxi/78786")
                    }
                });
                //设置枚举类型的值为string
                c.DescribeAllEnumsAsStrings();
                c.DocInclusionPredicate((docName, description) => true);
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "WebApi1.xml");
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //注册Consul 
            string ip = Configuration["service:ip"];
            string port = Configuration["service:port"];
            string serviceName = Configuration["service:name"];
            string serviceId = serviceName + Guid.Parse(webApi1Id);
            using (var consulClient = new ConsulClient(ConsulConfig))
            {
                AgentServiceRegistration asr = new AgentServiceRegistration
                {
                    Address = ip,
                    Port = Convert.ToInt32(port),
                    ID = serviceId,
                    Name = serviceName,
                    Check = new AgentServiceCheck
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                        HTTP = $"http://{ip}:{port}/api/Health",
                        Interval = TimeSpan.FromSeconds(10),
                        Timeout = TimeSpan.FromSeconds(5),
                    },
                };
                Console.WriteLine($"{asr.Check.HTTP}");
                consulClient.Agent.ServiceRegister(asr).Wait();
            }

            //注销Consul 
            appLifetime.ApplicationStopped.Register(() =>
            {
                using (var consulClient = new ConsulClient(ConsulConfig))
                {
                    Console.WriteLine("应用退出，开始从consul注销");
                    consulClient.Agent.ServiceDeregister(serviceId).Wait();
                }
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            // app.UseHttpsRedirection();
            app.UseMvc();
        }

        //Consul 配置委托
        private void ConsulConfig(ConsulClientConfiguration config)
        {
            config.Address = new Uri("http://127.0.0.1:8500"); //Demo硬编码Consul的地址
            config.Datacenter = "dc1";
        }
    }
}
