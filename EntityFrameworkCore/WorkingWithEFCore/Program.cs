using Northwind.EntityModels; // To use Northwind.

//using NorthwindDb db = new();
//WriteLine($"Provider: {db.Database.ProviderName}");
// Disposes the database context.

ConfigureConsole();
//QueryingCategories();

//FilteredIncludes();
//QueryingProducts();

//GettingOneProduct();

//QueryingWithLike();

//GetRandomProduct();

//LazyLoadingWithNoTracking();

// Add new product
//var resultAdd = AddProduct(categoryId: 6, productName: "Bob's Burgers", price: 500M, stock: 72);
//if (resultAdd.affected == 1)
//{
//    WriteLine($"Add product successful with ID: {resultAdd.productId}.");
//}
//ListProducts(new[] { resultAdd.productId });

// Update product
//var resultUpdate = IncreaseProductPrice("Bob", 20M);
//if (resultUpdate.affected == 1)
//{
//    WriteLine($"Increase price success for ID: {resultUpdate.productId}");
//}
//ListProducts(new[] { resultUpdate.productId });

// Delete product
//WriteLine("About to delete all products whose name starts with Bob.");
//Write("Press Enter to continue or any other key to exit: ");
//if (ReadKey(true).Key == ConsoleKey.Enter)
//{
//    int deleted = DeleteProducts("Bob");
//    WriteLine($"{deleted} product(s) were deleted.");
//}
//else
//{
//    WriteLine("Delete was canceled.");
//}

// Update product better
//var resultUpdateBetter = IncreaseProductPriceBetter("Bob", 20M);
//if (resultUpdateBetter.affected > 0)
//{
//    WriteLine("Increase product price successful.");
//}
//ListProducts(resultUpdateBetter.productIds);

// Delete product better
WriteLine("About to delete all products whose name starts with Bob.");
Write("Press Enter to continue or any other key to exit: ");
if (ReadKey(true).Key == ConsoleKey.Enter)
{
    int deleted = DeleteProductsBetter("Bob");
    WriteLine($"{deleted} product(s) were deleted.");
}
else
{
    WriteLine("Delete was canceled.");
}
