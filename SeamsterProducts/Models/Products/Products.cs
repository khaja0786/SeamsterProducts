using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seamster_Products.Models.Product
{
    public class Products
    {
        // internal object ProductId;

        [JsonProperty("ProductName")]
        public string ProductName { get; set; }

        [JsonProperty("ProductId")]
        public string ProductId { get; set; }

        [JsonProperty("ProductType")]
        public string ProductType { get; set; }

        [JsonProperty("id")]
        public string ProductGuid { get; set; }

        [JsonProperty("ProductPrice")]
        public int ProductPrice { get; set; }

    }
    public class Product
    {
        public Product()
        {
            LstProducts = new List<Product>();
        }
        public List<Product> LstPatient { get; set; }
        public List<Product> LstProducts { get; set; }
        public object ProductName { get; internal set; }

    }

    public class ProductTypes
    {
        [JsonProperty("ProductType")]
        public string ProductType { get; set; }
    }
}
