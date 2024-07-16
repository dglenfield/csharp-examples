using Northwind.Models; // To use Product.
using System.Text.Json.Serialization; // To use JsonSerializerContext.

namespace Northwind.Serialization;

[JsonSerializable(typeof(Product))]
[JsonSerializable(typeof(List<Product>))]
internal partial class NorthwindJsonSerializerContext : JsonSerializerContext
{

}
