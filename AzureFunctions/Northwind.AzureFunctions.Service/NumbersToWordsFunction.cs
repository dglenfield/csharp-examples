using Humanizer; // To use ToWords extension method.
using Microsoft.Azure.Functions.Worker; // To use [HttpTrigger].
using Microsoft.Azure.Functions.Worker.Http; // To use HttpResponseData.
using Microsoft.Extensions.Logging; // To use ILogger.
using System.Net;

namespace Northwind.AzureFunctions.Service;

public class NumbersToWordsFunction
{
    private readonly ILogger _logger;

    public NumbersToWordsFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<NumbersToWordsFunction>();
    }

    [Function(nameof(NumbersToWordsFunction))]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string? amount = req.Query["amount"];

        HttpResponseData response;

        if (long.TryParse(amount, out long number))
        {
            response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteString(number.ToWords());
        }
        else
        {
            response = req.CreateResponse(HttpStatusCode.BadRequest);
            response.WriteString($"Failed to parse: {amount}");
        }

        return response;
    }
}
