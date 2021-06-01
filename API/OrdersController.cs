using NorthwindWeb.DAL;
using NorthwindWeb.DTO;
using NorthwindWeb.Models;
using NorthwindWeb.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Serialization;
using System.Web;

namespace NorthwindWeb.API
{
    public class OrdersController : ApiController
    {
        OrderService _orderService;

        public OrdersController()
        {
            _orderService = new OrderService();
        }


        // GET api/<controller>
        public IEnumerable<string> Get()        
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public OrderDto Get(int id)
        {
            OrderDto order = _orderService.GetOrderDto(id);

            return order;
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}