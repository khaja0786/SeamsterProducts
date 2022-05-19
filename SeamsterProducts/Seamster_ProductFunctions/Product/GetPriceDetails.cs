using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Seamster_Products.Models.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Seamster_Products.Seamster_ProductFunctions.Product
{
    class GetPriceDetails
    {
        private static List<Products> lstProductNames;

        [FunctionName("GetPriceDetails")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetProductNamed/{ProductName}")] HttpRequest req, string ProductName,
            ILogger log)
        {
            //string ProductType=string.Empty;
            //declaring the response
            log.LogInformation("Calling Azure Function -- ProductGetId");
            // initialising Azure Cosomosdb database connection.
            ProductCosmoDBActivity objCosmosDBActivitiy = new ProductCosmoDBActivity();
            await objCosmosDBActivitiy.InitiateConnection();
            // retriving existing Product information based on ProductGuid and partition key i.e. ProductsId value
            lstProductNames = await objCosmosDBActivitiy.GetProductByName(ProductName);
            return new OkObjectResult(lstProductNames);
            // return new OkObjectResult("Kaja");
        }
    }
}
