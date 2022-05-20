using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Seamster_Products.Models.Order;
using System;
using System.Threading.Tasks;

namespace Seamster_Products.Seamster_ProductFunctions
{
    public static class GetOrderById
    {
        private static ItemResponse<Order> objorderResponse;

        [FunctionName("GetOrderById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "GetOrderById/{OrderGUID}/{partitionkey}")] HttpRequest req,
            string orderGUID, string partitionkey,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function - GetOrderById.");

            ItemResponse<Orders> objOrderResponse = null;
            log.LogInformation("Calling Azure Function -- GetOrderById");
            // initialising Azure Cosomosdb database connection.
            OrderCosmoDBActivity objCosmosDBActivitiy = new OrderCosmoDBActivity();
            await objCosmosDBActivitiy.InitiateConnection();
            // retriving existing Order information based on Order GUId and partition key i.e. OrderId Value
            objOrderResponse = await objCosmosDBActivitiy.GetOrderItem(orderGUID, partitionkey);

            if (objOrderResponse != null && objOrderResponse.Resource != null)

            {
                DateTime todaysDate = DateTime.Now, runningDate = DateTime.Now, Orderdate;

            }


            return new ObjectResult(objOrderResponse.Resource);
        }
    }
}
