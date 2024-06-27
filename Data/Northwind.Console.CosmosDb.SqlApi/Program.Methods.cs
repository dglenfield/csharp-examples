using System.Net; // To use HttpStatusCode.
using Microsoft.Azure.Cosmos; // To use CosmosClient and so on.
using Microsoft.Azure.Cosmos.Scripts; // To use StoredProcedureResponse and so on.
using Microsoft.EntityFrameworkCore; // To use Include extension method.
using Northwind.CosmosDb.Items; // To use ProductCosmos and so on.
using Northwind.EntityModels; // To use NorthwindContext and so on.

// This is defined in the default namespace, so it merges with the SDK-generated partial Program class.

partial class Program
{
    // To use Azure Cosmos DB in the local emulator.
    private static string endpointUri = "https://localhost:8081/";
    private static string primaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

    // To use Azure Cosmos DB in the cloud.
    //private static string account = "apps-services-book-dg";
    //private static string endpointUri = $"https://{account}.documents.azure.com:443/";
    //private static string primaryKey = "LGrx7H...gZw=="; // Use your key

    static async Task CreateCosmosResources()
    {
        SectionTitle("Creating Cosmos resources");

        try
        {
            using (CosmosClient client = new(endpointUri, primaryKey))
            {
                DatabaseResponse dbResponse = await client.CreateDatabaseIfNotExistsAsync("Northwind", throughput: 400);

                string status = dbResponse.StatusCode switch
                {
                    HttpStatusCode.OK => "exists",
                    HttpStatusCode.Created => "created",
                    _ => "unknown"
                };
                WriteLine("Database Id: {0}, Status: {1}.", dbResponse.Database.Id, status);

                IndexingPolicy indexingPolicy = new()
                {
                    IndexingMode = IndexingMode.Consistent,
                    Automatic = true, // Items are indexed unless explicitly excluded.
                    IncludedPaths = { new IncludedPath { Path = "/*" } }
                };

                ContainerProperties containerProperties = new(id: "Products", partitionKeyPath: "/productId")
                {
                    IndexingPolicy = indexingPolicy
                };

                ContainerResponse containerResponse =
                    await dbResponse.Database.CreateContainerIfNotExistsAsync(containerProperties, throughput: 1000);

                status = dbResponse.StatusCode switch
                {
                    HttpStatusCode.OK => "exists",
                    HttpStatusCode.Created => "created",
                    _ => "unknown"
                };
                WriteLine("Container Id: {0}, Status: {1}.", containerResponse.Container.Id, status);

                Container container = containerResponse.Container;

                ContainerProperties properties = await container.ReadContainerAsync();
                WriteLine($"  PartitionKeyPath: {properties.PartitionKeyPath}");
                WriteLine($"  LastModified: {properties.LastModified}");
                WriteLine("  IndexingPolicy.IndexingMode: {0}", properties.IndexingPolicy.IndexingMode);
                WriteLine("  IndexingPolicy.IncludedPaths: {0}",
                    string.Join(",", properties.IndexingPolicy.IncludedPaths.Select(path => path.Path)));
                WriteLine($"  IndexingPolicy: {properties.IndexingPolicy}");
            }
        }
        catch (HttpRequestException ex)
        {
            WriteLine($"Error: {ex.Message}");
            WriteLine("Hint: If you are using the Azure Cosmos Emulator then please make sure that it is running.");
        }
        catch (Exception ex) 
        {
            WriteLine("Error: {0} says {1}", ex.GetType(), ex.Message);
        }
    }

    static async Task CreateProductItems()
    {
        SectionTitle("Creating product items");

        double totalCharge = 0.0;

        try
        {
            using (CosmosClient client = new(endpointUri, primaryKey))
            {
                Container container = client.GetContainer(databaseId: "Northwind", containerId: "Products");

                using (NorthwindContext db = new())
                {
                    if (!db.Database.CanConnect())
                    {
                        WriteLine("Cannot connect to the SQL Server database to read products using database connection string: {0}", 
                            db.Database.GetConnectionString());
                        return;
                    }

                    ProductCosmos[] products = db.Products
                        // Get the related data for embedding.
                        .Include(p => p.Category).Include(p => p.Supplier)
                        // Filter any products with null category or supplier to avoid null warnings.
                        .Where(p => (p.Category != null) && (p.Supplier != null))
                        // Project the EF Core entities into Cosmos JSON types.
                        .Select(p => new ProductCosmos
                        {
                            id = p.ProductId.ToString(),
                            productId = p.ProductId.ToString(),
                            productName = p.ProductName,
                            quantityPerUnit = p.QuantityPerUnit,
                            // If the related category is null, store null, else store the category mapped to Cosmos model.
                            category = p.Category == null ? null : new CategoryCosmos
                            {
                                categoryId = p.Category.CategoryId,
                                categoryName = p.Category.CategoryName,
                                description = p.Category.Description
                            },
                            supplier = p.Supplier == null ? null : new SupplierCosmos
                            {
                                supplierId = p.Supplier.SupplierId,
                                companyName = p.Supplier.CompanyName,
                                contactName = p.Supplier.ContactName,
                                contactTitle = p.Supplier.ContactTitle,
                                address = p.Supplier.Address,
                                city = p.Supplier.City,
                                country = p.Supplier.Country,
                                postalCode = p.Supplier.PostalCode,
                                region = p.Supplier.Region,
                                phone = p.Supplier.Phone,
                                fax = p.Supplier.Fax,
                                homePage = p.Supplier.HomePage
                            },
                            unitPrice = p.UnitPrice,
                            unitsInStock = p.UnitsInStock,
                            reorderLevel = p.ReorderLevel,
                            unitsOnOrder = p.UnitsOnOrder,
                            discontinued = p.Discontinued
                        }).ToArray();

                    foreach (ProductCosmos product in products)
                    {
                        try
                        {
                            // Try to read the item to see if it exists.
                            ItemResponse<ProductCosmos> productResponse =
                                await container.ReadItemAsync<ProductCosmos>(product.id, new PartitionKey(product.id));

                            WriteLine("Item with id: {0} exists. Query consumed {1} RUs.",
                                productResponse.Resource.id, productResponse.RequestCharge);

                            totalCharge += productResponse.RequestCharge;
                        }
                        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                        {
                            // Create the item if it does not exist.
                            ItemResponse<ProductCosmos> productResponse = await container.CreateItemAsync(product);

                            WriteLine("Created item with id: {0}. Insert consumed {1} RUs.",
                                productResponse.Resource.id, productResponse.RequestCharge);

                            totalCharge += productResponse.RequestCharge;
                        }
                        catch (Exception ex) 
                        {
                            WriteLine("Error: {0} says {1}", ex.GetType(), ex.Message);
                        }
                    }
                }
            }
        }
        catch (HttpRequestException ex)
        {
            WriteLine($"Error: {ex.Message}");
            WriteLine("Hint: If you are using the Azure Cosmos Emulator then please make sure it is running.");
        }
        catch (Exception ex)
        {
            WriteLine("Error: {0} says {1}", ex.GetType(), ex.Message);
        }

        WriteLine("Total requests charge: {0:N2} RUs", totalCharge);
    }

    static async Task ListProductItems(string sqlText = "SELECT * FROM c")
    {
        SectionTitle("Listing product items");

        try
        {
            using (CosmosClient client = new(endpointUri, primaryKey))
            {
                Container container = client.GetContainer(databaseId: "Northwind", containerId: "Products");

                WriteLine("Running query: {0}", sqlText);

                QueryDefinition query = new(sqlText);

                using FeedIterator<ProductCosmos> resultsIterator =
                    container.GetItemQueryIterator<ProductCosmos>(query);

                if (!resultsIterator.HasMoreResults)
                {
                    WriteLine("No results found.");
                }

                while (resultsIterator.HasMoreResults) 
                {
                    FeedResponse<ProductCosmos> products = await resultsIterator.ReadNextAsync();

                    WriteLine("Status code: {0}, Request charge: {1} RUs.", products.StatusCode, products.RequestCharge);
                    WriteLine($"{products.Count} products found.");

                    foreach (ProductCosmos product in products) 
                    {
                        WriteLine("id: {0}, productName: {1}, unitPrice: {2:C}", 
                            product.id, product.productName, product.unitPrice);
                    }
                }
            }
        }
        catch (HttpRequestException ex)
        {
            WriteLine($"Error: {ex.Message}");
            WriteLine("Hint: If you are using the Azure Cosmos Emulator then please make sure it is running.");
        }
        catch (Exception ex)
        {
            WriteLine("Error: {0} says {1}", ex.GetType(), ex.Message);
        }
    }

    static async Task DeleteProductItems()
    {
        SectionTitle("Deleting product items");

        double totalCharge = 0.0;

        try
        {
            using (CosmosClient client = new(endpointUri, primaryKey))
            {
                Container container = client.GetContainer(databaseId: "Northwind", containerId: "Products");

                string sqlText = "SELECT * FROM c";

                WriteLine($"Running query: {sqlText}");

                QueryDefinition query = new(sqlText);

                using FeedIterator<ProductCosmos> resultsIterator = container.GetItemQueryIterator<ProductCosmos>(query);

                while (resultsIterator.HasMoreResults)
                {
                    FeedResponse<ProductCosmos> products = await resultsIterator.ReadNextAsync();

                    foreach (ProductCosmos product in products)
                    {
                        WriteLine("Delete id: {0}, productName: {1}", product.id, product.productName);

                        ItemResponse<ProductCosmos> response =
                            await container.DeleteItemAsync<ProductCosmos>(id: product.id, partitionKey: new(product.id));

                        WriteLine($"Status code: {response.StatusCode}, Request charge: {response.RequestCharge} RUs.");

                        totalCharge += response.RequestCharge;
                    }
                }
            }
        }
        catch (HttpRequestException ex)
        {
            WriteLine($"Error: {ex.Message}");
            WriteLine("Hint: If you are using the Azure Cosmos Emulator then please make sure it is running.");
        }
        catch (Exception ex) 
        {
            WriteLine($"Error: {ex.GetType()} says {ex.Message}");
        }

        WriteLine("Total requests charge: {0:N2} RUs", totalCharge);
    }

    static async Task CreateInsertProductStoredProcedure()
    {
        SectionTitle("Creating the insertProduct stored procedure");

        try
        {
            using (CosmosClient client = new(endpointUri, primaryKey))
            {
                Container container = client.GetContainer(databaseId: "Northwind", containerId: "Products");

                StoredProcedureResponse response = await container.Scripts.CreateStoredProcedureAsync(new StoredProcedureProperties
                {
                    Id = "insertProduct",
                    // __ means getContext().getCollection().
                    Body = """
                    function insertProduct(product) {
                        if (!product) throw new Error("product is undefined or null.");

                        tryInsert(product, callbackInsert);

                        function tryInsert(product, callbackFunction) {
                            var options = { disableAutomaticIdGeneration: false };

                            // __ is an alias for getContext().getCollection()
                            var isAccepted = __.createDocument(__.getSelfLink(), product, options, callbackFunction);

                            if (!isAccepted)
                                getContext().getResponse().setBody(0);
                        }

                        function callbackInsert(err, item, options) {
                            if (err) throw err;
                            getContext().getResponse().setBody(1);
                        }
                    }
                    """
                });

                WriteLine("Status code: {0}, Request charge: {1} RUs.", response.StatusCode, response.RequestCharge);
            }
        }
        catch (HttpRequestException ex)
        {
            WriteLine($"Error: {ex.Message}");
            WriteLine("Hint: If you are using the Azure Cosmos Emulator then please make sure it is running.");
        }
        catch (Exception ex) 
        {
            WriteLine($"Error: {ex.GetType()} says {ex.Message}");
        }
    }

    static async Task ExecuteInsertProductStoredProcedure()
    {
        SectionTitle("Executing the insertProduct stored procedure");

        try
        {
            using (CosmosClient client = new(endpointUri, primaryKey))
            {
                Container container = client.GetContainer(databaseId: "Northwind", containerId: "Products");

                string pid = "78";
                ProductCosmos product = new()
                {
                    id = pid, 
                    productId = pid, 
                    productName = "Barista's Chilli Jam", 
                    unitPrice = 12M, 
                    unitsInStock = 10
                };

                StoredProcedureExecuteResponse<string> response = 
                    await container.Scripts.ExecuteStoredProcedureAsync<string>("insertProduct", new PartitionKey(pid), [product]);

                WriteLine("Status code: {0}, Request charge: {1} RUs", response.StatusCode, response.RequestCharge);
            }
        }
        catch (HttpRequestException ex)
        {
            WriteLine($"Error: {ex.Message}");
            WriteLine("Hint: If you are using the Azure Cosmos Emulator then please make sure it is running.");
        }
        catch (Exception ex)
        {
            WriteLine($"Error: {ex.GetType()} says {ex.Message}");
        }
    }
}
