using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Seamster_Products.Models.Product
{
    public static class AllProducts
    {
        private static List<Products> lstAllProducts;

        [FunctionName("AllProducts")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "AllProducts")] HttpRequest req,
            ILogger log)
        {
            //declaring the response
            List<Product> lstProducts = null;
            log.LogInformation("Calling Azure Function -- ProductGetId");
            // initialising Azure Cosomosdb database connection.
            ProductCosmoDBActivity objCosmosDBActivitiy = new ProductCosmoDBActivity();
            await objCosmosDBActivitiy.InitiateConnection();
            // retriving existing Product information based on ProductGuid and partition key i.e. ProductsId value
            lstAllProducts = await objCosmosDBActivitiy.GetAllProducts();
            return new OkObjectResult(lstAllProducts);
            // return new OkObjectResult("Kaja");
        }
    }
}
