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
    public static class GetProductNameByType
    {
        private static List<Products> lstProductTypes;

        [FunctionName("GetProductName")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetProductName/{ProductType}")] HttpRequest req, string ProductType,
            ILogger log)
        {
            //string ProductType=string.Empty;
            //declaring the response
            log.LogInformation("Calling Azure Function -- ProductGetId");
            // initialising Azure Cosomosdb database connection.
            ProductCosmoDBActivity objCosmosDBActivitiy = new ProductCosmoDBActivity();
            await objCosmosDBActivitiy.InitiateConnection();
            // retriving existing Product information based on ProductGuid and partition key i.e. ProductsId value
            lstProductTypes = await objCosmosDBActivitiy.GetProductNameByType(ProductType);
            return new OkObjectResult(lstProductTypes);
            // return new OkObjectResult("Kaja");
        }
    }
}
