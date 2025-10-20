using Inspection.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();
await app.MigrateAndSeedData();

app.UserServices();


var env = app.Services.GetRequiredService<IWebHostEnvironment>();
await app.RunAsync();

