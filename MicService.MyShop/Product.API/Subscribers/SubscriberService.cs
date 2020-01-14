using System;
using DotNetCore.CAP;

namespace Product.API.Subscribers
{
    public class SubscriberService : ISubscriberService, ICapSubscribe
    {

        [CapSubscribe("myshop.product.addafter")]
        public void AddProductAfter(DateTime time)
        {
            Console.WriteLine($@"{DateTime.Now} Subscriber invoked, Info: {time}");
        }
    }
}
