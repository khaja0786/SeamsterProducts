using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Seamster_Products.Models.Product;
using Seamster_Products.Seamster_ProductFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seamster_Products.Seamster_ProductFunctions
{
    public static class ProductCreate
    {
        private static object objProducttDetails;

        [FunctionName("ProductCreate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "ProductCreate")] HttpRequest req,
            ILogger log)
        {
            // we are initialised required variables 
            string requestBody = null;
            Products objProductDetails = null;
            MyAzureFunctionResponse objResponse = new MyAzureFunctionResponse();
            ItemResponse<Products> objInsertResponse = null;
            log.LogInformation("Calling Azure Function -- ProductCreate");

            // we are reading or parsing the input request body
            requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            objProductDetails = JsonConvert.DeserializeObject<Products>(requestBody);

            if (objProductDetails != null)
            {
                // initialising Azure Cosomosdb database connection.
                ProductCosmoDBActivity objCosmosDBActivitiy = new ProductCosmoDBActivity();
                await objCosmosDBActivitiy.InitiateConnection();

                // saving new Product information.
                // System need to generate one unique value for "id" property while saving new items in container
                objProductDetails.ProductGuid = Guid.NewGuid().ToString();

                //var obj = await objCosmosDBActivitiy.GetAllProducts();
                // var a = obj.OrderByDescending(p => p.OrderId).Select(o => o.OrderId);

                objInsertResponse = await objCosmosDBActivitiy.SaveNewProductItem(objProductDetails);
                if (objInsertResponse == null)
                {
                    objResponse.ErrorCode = 100;
                    objResponse.Message = $"Error occured while inserting information of product- {objProductDetails.ProductName}";
                    log.LogInformation(objResponse.Message + "Error:" + objInsertResponse.StatusCode);
                }
                else
                {
                    objResponse.ErrorCode = 0;
                    objResponse.Message = "Successfully inserted information.";
                }

            }
            else
            {
                objResponse.ErrorCode = 100;
                objResponse.Message = "Failed to read or extract Student information from Request body due to bad data.";
                log.LogInformation("Failed to read or extract Student information from Request body due to bad data.");
            }
            return new OkObjectResult(objResponse);
        }
    }

    internal class MyAzureFunctionResponse
    {
        public int ErrorCode { get; internal set; }
        public string Message { get; internal set; }

        //public static implicit operator MyAzureFunctionResponse(AzureFunctionResponse v)
        //{
        //throw new NotImplementedException();
        //}
    }
}
