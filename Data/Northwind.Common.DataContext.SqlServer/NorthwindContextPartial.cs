using Microsoft.Data.SqlClient; // To use SqlConnectionStringBuilder.
using Microsoft.EntityFrameworkCore; // To use DbContext.

namespace Northwind.EntityModels;

public partial class NorthwindContext : DbContext
{
    private static readonly SetLastRefreshedInterceptor setLastRefreshedInterceptor = new();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) 
        {
            SqlConnectionStringBuilder builder = new();
            builder.DataSource = ".";
            builder.InitialCatalog = "Northwind";
            builder.TrustServerCertificate = true;
            builder.MultipleActiveResultSets = true;
            builder.ConnectTimeout = 3; // Because we want to fail faster. Default is 15 seconds.

            // If using Windows Integrated authentication.
            builder.IntegratedSecurity = true; 

            // If using SQL Server authentication
            //builder.UserID = Environment.GetEnvironmentVariable("MY_SQL_USR");
            //builder.Password = Environment.GetEnvironmentVariable("MY_SQL_PWD");

            optionsBuilder.UseSqlServer(builder.ConnectionString);
        }

        optionsBuilder.AddInterceptors(setLastRefreshedInterceptor);
    }
}
