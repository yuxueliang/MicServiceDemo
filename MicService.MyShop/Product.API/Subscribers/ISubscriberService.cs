using System;

namespace Product.API.Subscribers
{
    public interface ISubscriberService
    { 
        void AddProductAfter(DateTime datetime);
    }
}
