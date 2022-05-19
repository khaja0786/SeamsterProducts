using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seamster_Products.Models.Product
{
    public class ProductCosmoDBActivity
    {
        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string EndpointUri = "https://localhost:8081";
        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private string databaseId = "MyLearning";
        // private string containerId = "Container";
        private string containerId = "Product";

        public List<Product> ProductId { get; private set; }
        public Task<Product> objProductDetails { get; internal set; }

        public async Task InitiateConnection()
        {
            // Create a new instance of the Cosmos Client 
            //configuring Azure Cosmosdb sql api details
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            await CreateDatabaseAsync();
            await CreateContainerAsync();
        }

        //internal Task<ItemResponse<Products>> GetProductItem(string productGUID, int partitionkey)
        //{
        //    throw new NotImplementedException();
        //}

        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        }

        //internal Task<ItemResponse<Products>> SaveNewProductItem(Products objProductDetails)
        //{
        //    throw new NotImplementedException();
        //}


        /// <summary>
        /// Create the container if it does not exist. 
        /// Specifiy "/ProductName" as the partition key since we're storing Product information, to ensure good distribution of requests and storage.
        /// </summary>
        /// <returns></returns>
        private async Task CreateContainerAsync()
        {
            // Create a new container
            //this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/ProductName");
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/ProductId");
        }

        public async Task<ItemResponse<Products>> SaveNewProductItem(Products objProduct)
        {
            ItemResponse<Products> productResponse = null;
            try
            {
                //  studentResponse = await this.container.CreateItemAsync<Student>(objProduct, new PartitionKey(objProduct.Name));
                productResponse = await this.container.CreateItemAsync<Products>(objProduct, new PartitionKey(objProduct.ProductId));
            }
            catch (CosmosException ex)
            {
                throw ex;
            }
            return productResponse;
        }

        public async Task<ItemResponse<Products>> ModifyProductItem(Products objProduct)
        {
            ItemResponse<Products> ProductResponse = null;
            try
            {
                /* Note : Partition Key value should not change */
                // ProductResponse = await this.container.ReplaceItemAsync<Product>(objProduct, objProduct.ProductId, new PartitionKey(objProduct.Name));
                ProductResponse = await this.container.ReplaceItemAsync<Products>(objProduct, objProduct.ProductGuid,
                    new PartitionKey(objProduct.ProductId));
            }
            catch (CosmosException ex)
            {
                throw ex;
            }
            return ProductResponse;
        }


        public async Task<ItemResponse<Products>> GetProductItem(string ProductId, string partionKey)
        {
            ItemResponse<Products> ProductResponse = null;
            try
            {
                ProductResponse = await this.container.ReadItemAsync<Products>(ProductId, new PartitionKey(partionKey));
            }
            catch (CosmosException ex)
            {
                throw ex;
            }
            return ProductResponse;
        }
        public async Task<List<Products>> GetAllProducts()
        {
            var sqlQueryText = "SELECT * FROM c";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Products> queryResultSetIterator = this.container.GetItemQueryIterator<Products>(queryDefinition);

            List<Products> lstProducts = new List<Products>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Products> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                lstProducts = currentResultSet.Select(r => new Products()
                {
                    ProductId = r.ProductId,
                    ProductName = r.ProductName,
                    ProductType = r.ProductType,
                    ProductGuid = r.ProductGuid,
                    ProductPrice = r.ProductPrice,
                }).ToList();


            }
            return lstProducts;
        }

        public async Task<List<ProductTypes>> GetProductTypes()
        {
            List<ProductTypes> objListProductTypes = new List<ProductTypes>();

            var sqlQueryText = "SELECT c.ProductType FROM c";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<ProductTypes> queryResultSetIterator = this.container.GetItemQueryIterator<ProductTypes>(queryDefinition);

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<ProductTypes> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                objListProductTypes = currentResultSet.Select(r => new ProductTypes()
                {

                    ProductType = r.ProductType

                }).ToList();


            }
            return objListProductTypes;
        }

        public async Task<List<Products>> GetProductNameByType(string productType)
        {
            List<Products> objListProductNames = new List<Products>();

            var sqlQueryText = $"select * from c where c.ProductType ='{productType}'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Products> queryResultSetIterator = this.container.GetItemQueryIterator<Products>(queryDefinition);

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Products> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                objListProductNames = currentResultSet.Select(r => new Products()
                {

                    ProductName = r.ProductName,
                    ProductPrice = r.ProductPrice

                }).ToList();


            }
            return objListProductNames;
        }


        public async Task<List<Products>> GetProductByName(string ProductName)
        {
            List<Products> objListProductNames = new List<Products>();

            var sqlQueryText = $"select * from c where c.ProductName ='{ProductName}'";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Products> queryResultSetIterator = this.container.GetItemQueryIterator<Products>(queryDefinition);

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Products> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                objListProductNames = currentResultSet.Select(r => new Products()
                {

                    ProductName = r.ProductName,
                    ProductPrice = r.ProductPrice

                }).ToList();


            }
            return objListProductNames;
        }
    }

}

