﻿using System.Globalization; // To use CultureInfo.
using System.Text; // To enable Euro symbol output.

//OutputEncoding = Encoding.UTF8; // To enable Euro symbol output.

// Simulate French culture to test Euro currency symbol output.
//Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");

//await CreateCosmosResources();

//await CreateProductItems();

//await ListProductItems("SELECT p.id, p.productName, p.unitPrice FROM Items p WHERE p.category.categoryName = 'Beverages'");

//await DeleteProductItems();

//await CreateInsertProductStoredProcedure();

await ExecuteInsertProductStoredProcedure();
await ListProductItems("SELECT p.id, p.productName, p.unitPrice FROM Items p WHERE p.productId = '78'");
