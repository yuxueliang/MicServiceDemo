using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Msg.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSController : ControllerBase
    {


        // GET: api/Product
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string name = this.User.Identity.Name;//读取的就是"Name"这个特殊的 Claims 的值
            string userId = this.User.FindFirst("UserId").Value;
            string realName = this.User.FindFirst("RealName").Value;
            string email = this.User.FindFirst("Email").Value;
            Console.WriteLine($"name={name},userId={userId},realName={realName},email={email}");

            return new string[] { "value1", "value2" };
        }

        // GET: api/Product/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Product
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }



        // PUT: api/Product/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        
    }
}
