using Microsoft.EntityFrameworkCore; // To use ToListAsync<T>.

namespace Northwind.Blazor.Services;

public class NorthwindServiceServerSide : INorthwindService
{
    private readonly NorthwindContext _db;

    public NorthwindServiceServerSide(NorthwindContext db)
    {
        _db = db;
    }

    public Task<Customer> CreateCustomerASync(Customer c)
    {
        _db.Customers.Add(c);
        _db.SaveChangesAsync();
        return Task.FromResult(c);
    }

    public Task DeleteCustomerASync(string id)
    {
        Customer? customer = _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == id).Result;

        if (customer == null)
        {
            return Task.CompletedTask;
        }

        _db.Customers.Remove(customer);
        return _db.SaveChangesAsync();
    }

    public Task<Customer?> GetCustomerASync(string id)
    {
        return _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);
    }

    public Task<List<Customer>> GetCustomersASync()
    {
        return _db.Customers.ToListAsync();
    }

    public Task<List<Customer>> GetCustomersASync(string country)
    {
        return _db.Customers.Where(c => c.Country == country).ToListAsync();
    }

    public Task<Customer> UpdateCustomerASync(Customer c)
    {
        _db.Entry(c).State = EntityState.Modified;
        _db.SaveChangesAsync();
        return Task.FromResult(c);
    }
}
