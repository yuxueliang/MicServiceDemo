using System;
using DotNetCore.CAP;

namespace Product.API.Subscribers
{
    public class SubscriberService : ICapSubscribe
    {

        [CapSubscribe("myshop.product.addafter", Group = "group1")]
        public void AddProductAfter(DateTime time)
        {
            Console.WriteLine($@"AddProductAfter-----{DateTime.Now} Subscriber invoked, Info: {time}");
        }

        [CapSubscribe("myshop.product.addafter", Group = "group2")]
        public void AddProductAfter1(DateTime time)
        {
            Console.WriteLine($@"AddProductAfter1 ---{DateTime.Now} Subscriber invoked, Info: {time}");
        }


    }

    
}
