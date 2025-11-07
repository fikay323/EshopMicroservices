var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();

var app = builder.Build();

app.UseApiServices();

app.Run();