using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Msg.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:9500";//identity server 地址             
                    options.RequireHttpsMetadata = false;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();//认证

            app.UseAuthorization();//授权


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


            RegeditService(applicationLifetime);

           
            
        }

        /// <summary>
        /// 注入consul服务
        /// </summary>
        public static async void RegeditService(IHostApplicationLifetime applicationLifetime)
        {
            string Address = "localhost";//服务ip地址
            int Port = 5001;//服务端口
            var serviceName = "MsgService";
            var serviceId = serviceName + Guid.NewGuid();
            using (var client = new ConsulClient())
            {
                var result = await client.Agent.ServiceRegister(new AgentServiceRegistration()
                {
                    ID = serviceId,//服务编号保证不重复
                    Name = serviceName,//服务的名称 集群使用consul
                    Address = Address,//服务ip地址
                    Port = Port,//服务端口
                    Check = new AgentServiceCheck //健康检查
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后反注册
                        Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔（定时检查服务是否健康）
                        HTTP = "http://" + Address + ":" + Port + "/api/health",//健康检查地址
                        Timeout = TimeSpan.FromSeconds(5)//服务的注册时间
                    }
                });
                string ss = result.StatusCode.ToString();
            }


            //程序正常退出的时候从 Consul 注销服务
            //要通过方法参数注入 IApplicationLifetime
            //程序结束的时候会调用这个方法
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                using (var client = new ConsulClient())
                {
                    client.Agent.ServiceDeregister(serviceId).Wait();
                }
            });
        }

     
    }
}
