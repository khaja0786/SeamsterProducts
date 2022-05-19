using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Seamster_Products.Models;
using Seamster_Products.Models.Order;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seamster_Products.Seamster_ProductFunctions
{
    public static class OrderCreate
    {
        private static object objOrderDetails;

        [FunctionName("OrderCreate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "OrderCreate")] HttpRequest req,
            ILogger log)
        {
            // we are initialised required variables 
            string requestBody = null;
            // Orders objOrderDetails = null;
            MyAzureFunctionResponse objResponse = new MyAzureFunctionResponse();
            ItemResponse<Orders> objInsertResponse = null;
            log.LogInformation("Calling Azure Function -- OrderCreate");

            // we are reading or parsing the input request body
            requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Orders objOrderDetails = JsonConvert.DeserializeObject<Orders>(requestBody);


            if (objOrderDetails != null)
            {
                OrderCosmoDBActivity objCosmosDBActivitiy = new OrderCosmoDBActivity();

                await objCosmosDBActivitiy.InitiateConnection();
                objOrderDetails.OrderId = await objCosmosDBActivitiy.GetLatestOrderId();

                await objCosmosDBActivitiy.InitiateConnection();
                objOrderDetails.OrderNumber = await objCosmosDBActivitiy.GetLatestOrderNumber();

                await objCosmosDBActivitiy.InitiateConnection();
                objOrderDetails.Orderdate = DateTime.Now;
                if (objOrderDetails.ProductType == "Tommy")
                    objOrderDetails.NextOrderdate = objOrderDetails.Orderdate.AddDays(1);
                if (objOrderDetails.ProductType == "Hilfiger")
                    objOrderDetails.NextOrderdate = objOrderDetails.Orderdate.AddDays(7);
                if (objOrderDetails.ProductType == "Us Polo")
                    objOrderDetails.NextOrderdate = objOrderDetails.Orderdate.AddDays(30);
                if (objOrderDetails.ProductType == "Levis")
                    objOrderDetails.NextOrderdate = objOrderDetails.Orderdate.AddDays(365);
                // if (objOrderDetails.ProductType == "Levi's")
                //  objOrderDetails.Orderdate = objOrderDetails.Orderdate.AddDays(180);





                objOrderDetails.OrderGuid = Guid.NewGuid().ToString();

                objInsertResponse = await objCosmosDBActivitiy.SaveNewOrderItem(objOrderDetails);
                if (objInsertResponse == null)
                {
                    objResponse.ErrorCode = 100;
                    objResponse.Message = $"Error occured while inserting information of order- {objOrderDetails.OrderId}";
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
}
