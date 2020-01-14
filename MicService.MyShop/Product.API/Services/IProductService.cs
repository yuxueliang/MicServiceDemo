using System;
using DotNetCore.CAP;
using Microsoft.Data.SqlClient;

namespace Product.API.Services
{
    public interface IProductService
    {
        void Add();
    }


    public class ProductService:IProductService
    {
        private readonly ICapPublisher _capBus;

        public ProductService(ICapPublisher capBus)
        {
            _capBus = capBus;
        }

        public void Add()
        {
            using (var connection = new SqlConnection("Data Source = 192.168.189.128;Initial Catalog = TEST_DB;User Id = sa;Password = sa123456;"))
            {
                using (var transaction = connection.BeginTransaction(_capBus, autoCommit: true))
                {
                    //your business logic code

                    _capBus.Publish("myshop.product.addafter", DateTime.Now);
                }
            }
        }
    }
}
