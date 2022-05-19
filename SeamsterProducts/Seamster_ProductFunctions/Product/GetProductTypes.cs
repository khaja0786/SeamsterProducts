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
    public static class GetProductTypes
    {
        private static List<ProductTypes> lstProductTypes;

        [FunctionName("GetProductTypes")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetProductTypes")] HttpRequest req,
            ILogger log)
        {
            //declaring the response
            log.LogInformation("Calling Azure Function -- ProductGetId");
            // initialising Azure Cosomosdb database connection.
            ProductCosmoDBActivity objCosmosDBActivitiy = new ProductCosmoDBActivity();
            await objCosmosDBActivitiy.InitiateConnection();
            // retriving existing Product information based on ProductGuid and partition key i.e. ProductsId value
            lstProductTypes = await objCosmosDBActivitiy.GetProductTypes();
            return new OkObjectResult(lstProductTypes);
            // return new OkObjectResult("Kaja");
        }
    }
}
