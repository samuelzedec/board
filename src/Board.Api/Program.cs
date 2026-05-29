using Board.Api.Setups;

var builder = WebApplication.CreateBuilder(args);
builder.Configure();

var app = builder.Build();
await app.ConfigureAsync();

await app.RunAsync();