using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Seamster_Products.Models.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Seamster_Products.Seamster_ProductFunctions
{
    public static class AllOrders
    {
        [FunctionName("AllOrders")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "AllOrders")] HttpRequest req,
            ILogger log)
        {
            //declaring the response
            List<Orders> lstOrders = null;
            log.LogInformation("Calling Azure Function -- OrderGetId");
            // initialising Azure Cosomosdb database connection.
            OrderCosmoDBActivity objCosmosDBActivitiy = new OrderCosmoDBActivity();
            await objCosmosDBActivitiy.InitiateConnection();
            // retriving existing Order information based on OrderGuid and partition key i.e. OrderId value
            lstOrders = await objCosmosDBActivitiy.GetAllOrders();
            return new OkObjectResult(lstOrders);
        }
    }
}
