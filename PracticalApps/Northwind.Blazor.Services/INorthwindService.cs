using Northwind.EntityModels; // To use Customer.

namespace Northwind.Blazor.Services;

public interface INorthwindService
{
    Task<Customer> CreateCustomerASync(Customer c);
    Task DeleteCustomerASync(string id);
    Task<Customer?> GetCustomerASync(string id);
    Task<List<Customer>> GetCustomersASync();
    Task<List<Customer>> GetCustomersASync(string country);
    Task<Customer> UpdateCustomerASync(Customer c);
}
