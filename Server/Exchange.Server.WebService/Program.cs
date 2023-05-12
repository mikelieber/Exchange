using Exchange.Server.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGenerator(builder.Configuration);
builder.Services.AddTransmitter(builder.Configuration);

var app = builder.Build();

app.Run();