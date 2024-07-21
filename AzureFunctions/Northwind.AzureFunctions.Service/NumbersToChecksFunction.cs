using Humanizer; // To use ToWords extension method.
using Microsoft.Azure.Functions.Worker; // To use [Function] and so on.
using Microsoft.Azure.Functions.Worker.Http; // To use HttpRequestData.
using Microsoft.Extensions.Logging; // To use ILogger.

namespace Northwind.AzureFunctions.Service;

public class NumbersToChecksFunction
{
    private readonly ILogger _logger;

    public NumbersToChecksFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<NumbersToChecksFunction>();
    }

    [Function(nameof(NumbersToChecksFunction))]
    public NumbersToChecksOutput Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData request)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string? amount = request.Query["amount"];

        if (long.TryParse(amount, out long number))
        {
            string numberAsWords = number.ToWords();

            var response = request.CreateResponse(System.Net.HttpStatusCode.OK);
            response.WriteString(numberAsWords);

            return new NumbersToChecksOutput() { ChecksQueueOutput = numberAsWords, HttpResponse = response };
        }

        var errorResponse = request.CreateResponse(System.Net.HttpStatusCode.BadRequest);
        errorResponse.WriteString($"Failed to parse: {amount}");

        return new NumbersToChecksOutput() { HttpResponse = errorResponse };
    }
}

public class NumbersToChecksOutput
{
    [QueueOutput("checksQueue")]
    public string? ChecksQueueOutput { get; set; }

    public HttpResponseData? HttpResponse { get; set; }
}