using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Northwind.EntityModels;

namespace Northwind.Web.Pages;

public class WebApiTestModel : PageModel
{
    [BindProperty]
    public List<Customer>? Customers { get; set; }

    private readonly IHttpClientFactory _clientFactory;

    public WebApiTestModel(IHttpClientFactory httpClientFactory)
    {
        _clientFactory = httpClientFactory;
    }

    public async Task OnGetAsync()
    {
        ViewData["Title"] = "All Customers Worldwide";

        string uri = "api/customers";

        HttpClient client = _clientFactory.CreateClient(name: "Northwind.WebApi");

        HttpRequestMessage request = new(HttpMethod.Get, uri);

        HttpResponseMessage response = await client.SendAsync(request);

        IEnumerable<Customer>? model = await response.Content.ReadFromJsonAsync<IEnumerable<Customer>>();
        Customers = model?.ToList();
    }
}
