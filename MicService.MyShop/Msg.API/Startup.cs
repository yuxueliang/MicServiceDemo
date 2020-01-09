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
                    options.Authority = "http://localhost:9500";//identity server ��ַ             
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

            app.UseAuthentication();//��֤

            app.UseAuthorization();//��Ȩ


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


            RegeditService(applicationLifetime);

           
            
        }

        /// <summary>
        /// ע��consul����
        /// </summary>
        public static async void RegeditService(IHostApplicationLifetime applicationLifetime)
        {
            string Address = "localhost";//����ip��ַ
            int Port = 5001;//����˿�
            var serviceName = "MsgService";
            var serviceId = serviceName + Guid.NewGuid();
            using (var client = new ConsulClient())
            {
                var result = await client.Agent.ServiceRegister(new AgentServiceRegistration()
                {
                    ID = serviceId,//�����ű�֤���ظ�
                    Name = serviceName,//��������� ��Ⱥʹ��consul
                    Address = Address,//����ip��ַ
                    Port = Port,//����˿�
                    Check = new AgentServiceCheck //�������
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//����������ú�ע��
                        Interval = TimeSpan.FromSeconds(10),//�������ʱ���������߳�Ϊ�����������ʱ�������Ƿ񽡿���
                        HTTP = "http://" + Address + ":" + Port + "/api/health",//��������ַ
                        Timeout = TimeSpan.FromSeconds(5)//�����ע��ʱ��
                    }
                });
                string ss = result.StatusCode.ToString();
            }


            //���������˳���ʱ��� Consul ע������
            //Ҫͨ����������ע�� IApplicationLifetime
            //���������ʱ�������������
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
