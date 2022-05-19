using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Seamster_Products.Models.Order
{
    public class Orders
    {
        [JsonProperty("OrderId")]
        public string OrderId { get; set; }

        // public object id { get; internal set; }

        [JsonProperty("OrderNumber")]
        public string OrderNumber { get; set; }

        [JsonProperty("Orderdate")]
        public DateTime Orderdate { get; set; }


        [JsonProperty("ProductType")]
        public string ProductType { get; set; }


        //[JsonProperty("OrderType")]
        //public string OrderType { get; set; }

        [JsonProperty("id")]
        public string OrderGuid { get; set; }

        [JsonProperty("NextOrderdate")]
        public DateTime NextOrderdate { get; set; }


    }
    public class Order
    {
        // internal object orderName;

        public Order()
        {
            LstOrder = new List<Order>();
        }
        public List<Order> LstOrder { get; set; }

        public string ProductType { get; set; }
        public string OrderNumber { get; set; }
        public DateTime Orderdate { get; set; }
        public string OrderId { get; set; }
        public string OrderGuid { get; set; }


    }
}
