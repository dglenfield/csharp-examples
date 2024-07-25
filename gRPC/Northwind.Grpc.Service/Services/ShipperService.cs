using Grpc.Core; // To use ServerCallContext.
using Microsoft.Data.SqlClient; // To use SqlConnection and so on.
using Northwind.EntityModels; // To use NorthwindContext.
using System.Data; // To use CommandType.
using ShipperEntity = Northwind.EntityModels.Shipper;

namespace Northwind.Grpc.Service.Services;

public class ShipperService : Shipper.ShipperBase
{
    private readonly ILogger<ShipperService> _logger;
    private readonly NorthwindContext _db;

    public ShipperService(ILogger<ShipperService> logger, NorthwindContext db)
    {
        _logger = logger;
        _db = db;
    }

    public override async Task<ShipperReply?> GetShipper(
        ShipperRequest request, ServerCallContext context)
    {
        _logger.LogCritical($"This request has a deadline of {context.Deadline:T}. It is now {DateTime.UtcNow:T}.");
        //await Task.Delay(TimeSpan.FromSeconds(5));

        // We cannot use EF Core in a native AOT compiled project.
        //ShipperEntity? shipper = await _db.Shippers.FindAsync(request.ShipperId);

        SqlConnectionStringBuilder builder = new();
        builder.InitialCatalog = "Northwind";
        builder.MultipleActiveResultSets = true;
        builder.Encrypt = true;
        builder.TrustServerCertificate = true;
        builder.ConnectTimeout = 10; // Default is 30 seconds.
        builder.DataSource = "."; // To use local SQL Server.
        builder.IntegratedSecurity = true;

        SqlConnection connection = new(builder.ConnectionString);

        await connection.OpenAsync();

        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "SELECT ShipperId, CompanyName, Phone FROM Shippers WHERE ShipperId = @id";
        cmd.Parameters.AddWithValue("id", request.ShipperId);

        SqlDataReader r = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow);

        ShipperReply? shipper = null;

        // Read the expected single row.
        if (await r.ReadAsync()) 
        {
            shipper = new()
            {
                ShipperId = r.GetInt32("ShipperId"),
                CompanyName = r.GetString("CompanyName"),
                Phone = r.GetString("Phone")
            };
        }

        await r.CloseAsync();

        return shipper;

        //return shipper is null ? null : ToShipperReply(shipper);
    }

    // A mapping method to convert from a shipper in the entity model
    // to a gRPC ShipperReply.
    private ShipperReply ToShipperReply(ShipperEntity shipper) 
    {
        return new ShipperReply
        {
            ShipperId = shipper.ShipperId,
            CompanyName = shipper.CompanyName,
            Phone = shipper.Phone
        };
    }
}
