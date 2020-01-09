using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Consul;

namespace Test
{

    class Program
    {
        public class Person
        {
            public long Id { get; set; }
        }
        static void Main(string[] args)
        {
            var ids = new long[]{ 1,2,5};
            IEnumerable<Person> persons = ids.Select(o => new Person(){ }).ToList();
            foreach (var person in persons)
            {
                person.Id = 10;
            }







           // ConsumeServices();
            Console.ReadKey();
        }

        /// <summary>
        /// 获取Consul下的所有注册的服务
        /// </summary>
        private static void GetServices()
        {
            using (var consulClient = new ConsulClient(c => c.Address = new Uri("http://127.0.0.1:8500")))
            {
                var services = consulClient.Agent.Services().Result.Response;
                foreach (var service in services.Values)
                {
                    Console.WriteLine($"id={service.ID},name={service.Service},ip={service.Address},port={service.Port}");
                }
            }
        }

        /// <summary>
        /// 找到MsgService的相关信息地址和端口
        /// </summary>
        private static void ConsumeServices()
        {
            using (var consulClient = new ConsulClient(c => c.Address = new Uri("http://127.0.0.1:8500")))
            {
                var services = consulClient.Agent.Services().Result.Response.Values.Where(s => s.Service.Equals("MsgService", StringComparison.OrdinalIgnoreCase));
                if (!services.Any())
                {
                    Console.WriteLine("找不到服务的实例");
                }
                else
                {
                    var service = services.ElementAt(Environment.TickCount % services.Count());
                    Console.WriteLine($"{service.Address}:{service.Port}");
                }
            }
        }
    }
}
