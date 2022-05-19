using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Seamster_Products.Models.Order;
using Seamster_Products.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Seamster_Products.Seamster_ProductFunctions
{
    public static class GetProductById
    {
        [FunctionName("GetProductById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "GetProductById/{productGUID}/{partitionkey}")] HttpRequest req,
            string productGUID, string partitionkey,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function - GetProductById.");

            ItemResponse<Products> objProductResponse = null;
            log.LogInformation("Calling Azure Function -- GetProductById");
            // initialising Azure Cosomosdb database connection.
            ProductCosmoDBActivity objCosmosDBActivitiy = new ProductCosmoDBActivity();
            await objCosmosDBActivitiy.InitiateConnection();
            // retriving existing patient information based on Product GUId and partition key i.e. patientId value
            objProductResponse = await objCosmosDBActivitiy.GetProductItem(productGUID, partitionkey);

            if (objProductResponse != null && objProductResponse.Resource != null)
            {
                OrderCosmoDBActivity objOrderCosmosDBActivitiy = new OrderCosmoDBActivity();
                await objOrderCosmosDBActivitiy.InitiateConnection();
                // objProductResponse.Resource.ProductId = await objOrderCosmosDBActivitiy.GetOrderIdByProductId(objProductResponse.Resource.ProductId);
            }
            return new OkObjectResult(objProductResponse.Resource);
        }
    }
}
