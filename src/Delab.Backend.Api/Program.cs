using Delab.Backend.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddApiConfiguration(configuration);

builder.Services.ResolveDependencies();


var app = builder.Build();

app.UseApiConfiguration(configuration);

app.Run();
