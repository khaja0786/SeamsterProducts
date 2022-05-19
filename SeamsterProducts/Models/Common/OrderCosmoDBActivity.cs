using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seamster_Products.Models.Order
{
    public class OrderCosmoDBActivity
    {


        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string EndpointUri = "https://localhost:8081";
        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we be create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private string databaseId = "MyLearning";

        // private string containerId = "Container";
        private string containerId = "Order";
        private object objOrder;


        public List<Order> OrderId { get; private set; }
        public Task<Order> objOrderDetails { get; internal set; }


        public ItemResponse<Order> OrderResponse { get; private set; }
        public string Id { get; private set; }

        public async Task InitiateConnection()
        {
            // Create a new instance of the Cosmos Client 
            //configuring Azure Cosmosdb sql api details
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            await CreateDatabaseAsync();
            await CreateContainerAsync();
        }


        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        }


        private async Task CreateContainerAsync()
        {
            // Create a new container
            //this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/Orderid");
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/OrderId");
        }



        public async Task<ItemResponse<Orders>> SaveNewOrderItem(Orders objOrderDetails)
        {
            ItemResponse<Orders> orderResponse = null;
            try
            {
                orderResponse = await this.container.CreateItemAsync<Orders>(objOrderDetails, new PartitionKey(objOrderDetails.OrderId));
            }
            catch (CosmosException ex)
            {
                throw ex;
            }
            return orderResponse;
        }

        public async Task<ItemResponse<Orders>> ModifyOrderItem(Orders objOrder)
        {
            ItemResponse<Orders> OrderResponse = null;
            try
            {
                OrderResponse = await this.container.ReplaceItemAsync<Orders>(objOrder, objOrder.OrderGuid, new PartitionKey(objOrder.OrderId));
            }
            catch (CosmosException ex)
            {
                throw ex;
            }
            return OrderResponse;
        }


        public async Task<ItemResponse<Orders>> GetOrderItem(string Id, string partionKey)
        {
            ItemResponse<Orders> OrderResponse = null;
            try
            {
                OrderResponse = await this.container.ReadItemAsync<Orders>(Id, new PartitionKey(partionKey));
            }
            catch (CosmosException ex)
            {
                throw ex;
            }
            return OrderResponse;
        }
        public async Task<List<Orders>> GetAllOrders()
        {
            var sqlQueryText = "SELECT * FROM c";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Orders> queryResultSetIterator = this.container.GetItemQueryIterator<Orders>(queryDefinition);

            List<Orders> lstOrders = new List<Orders>();

            while (queryResultSetIterator.HasMoreResults)
            {
                try
                {
                    FeedResponse<Orders> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    lstOrders = currentResultSet.Select(r => new Orders()
                    {
                        OrderId = r.OrderId,
                        OrderNumber = r.OrderNumber,
                        ProductType = r.ProductType,
                        Orderdate = r.Orderdate,
                        OrderGuid = r.OrderGuid
                    }).ToList();

                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);

                }

            }
            return lstOrders;
        }

        public async Task<string> GetLatestOrderId()
        {
            var sqlScript = "SELECT  * from c order by c.OrderId desc OFFSET 0 LIMIT 1";
            QueryDefinition queryDefinition = new QueryDefinition(sqlScript);
            FeedIterator<Order> queryResultSetIterator = this.container.GetItemQueryIterator<Order>(queryDefinition);

            string orderId = string.Empty;
            int latestOrderId = 0;
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Order> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                latestOrderId = Convert.ToInt32(currentResultSet.Select(r => r.OrderId).FirstOrDefault());
                if (latestOrderId != 0)
                {
                    orderId = Convert.ToString(latestOrderId + 1);
                }
            }
            return orderId;
        }

        public async Task<string> GetLatestOrderNumber()
        {
            var sqlScript = "SELECT  * from c order by c.OrderNumber desc OFFSET 0 LIMIT 1";
            QueryDefinition queryDefinition = new QueryDefinition(sqlScript);
            FeedIterator<Order> queryResultSetIterator = this.container.GetItemQueryIterator<Order>(queryDefinition);

            string orderNumber = string.Empty;
            int latestOrderNumber = 0;
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Order> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                latestOrderNumber = Convert.ToInt32(currentResultSet.Select(r => r.OrderNumber).FirstOrDefault());
                if (latestOrderNumber != 0)
                {
                    orderNumber = Convert.ToString(latestOrderNumber + 1);
                }
            }
            return orderNumber;
        }

        public async Task<DateTime> GetLatestOrderdate()
        {
            var sqlScript = "SELECT  * from c order by c.Orderdate desc OFFSET 0 LIMIT 1";
            QueryDefinition queryDefinition = new QueryDefinition(sqlScript);
            FeedIterator<Order> queryResultSetIterator = this.container.GetItemQueryIterator<Order>(queryDefinition);

            DateTime orderdate = DateTime.UtcNow;
            int latestOrderdate = 0;
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Order> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                latestOrderdate = Convert.ToInt32(currentResultSet.Select(r => r.Orderdate).FirstOrDefault());
            }
            return orderdate;
        }

        public async Task<string> GetOrderIdByProductId(string productId)
        {
            var sqlQueryText = $"select * from c where c.ProductId ='{productId}'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Order> queryResultSetIterator = this.container.GetItemQueryIterator<Order>(queryDefinition);

            string lstOrders = string.Empty;

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Order> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                lstOrders = string.Join(",", currentResultSet.Select(r => r.OrderId).ToList<string>());
            }
            return lstOrders;

        }
    }
}
