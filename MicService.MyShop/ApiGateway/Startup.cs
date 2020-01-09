using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace ApiGateway
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

            //ָ��Identity Server����Ϣ
            Action<IdentityServerAuthenticationOptions> isaOptMsg = o =>
            {
                o.Authority = "http://localhost:9500";
                o.ApiName = "MsgAPI";//Ҫ���ӵ�Ӧ�õ�����
                o.RequireHttpsMetadata = false;
                o.SupportedTokens = SupportedTokens.Both;
                o.ApiSecret = "123321";//��Կ
            };
            Action<IdentityServerAuthenticationOptions> isaOptProduct = o =>
            {
                o.Authority = "http://localhost:9500";
                o.ApiName = "ProductAPI";//Ҫ���ӵ�Ӧ�õ�����
                o.RequireHttpsMetadata = false;
                o.SupportedTokens = SupportedTokens.Both;
                o.ApiSecret = "123321";//��Կ            
            };
            services.AddAuthentication()
                //�������ļ���ʹ��ChatKey������AuthenticationProviderKey=MsgKey
                //��·�ɹ���ʹ�����µ���֤��ʽ
                .AddIdentityServerAuthentication("MsgKey", isaOptMsg).AddIdentityServerAuthentication("ProductKey", isaOptProduct); 



            services.AddOcelot().AddConsul();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
         
            app.UseOcelot().Wait();//��Ҫ����дWait
        }
    }
}
