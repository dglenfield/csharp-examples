using Northwind.EntityModels; // To use AddNorthwindContext method.
using Northwind.GraphQL.Service; // To use Query.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNorthwindContext();

builder.Services
    .AddGraphQLServer()
    .AddFiltering()
    .AddSorting()
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions()
    .RegisterDbContext<NorthwindContext>()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

var app = builder.Build();

app.UseWebSockets();

app.MapGet("/", () => "Navigate to: https://localhost:5121/graphql");

app.MapGraphQL();

app.Run();
