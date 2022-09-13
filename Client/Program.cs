using Client.Services.Analyzer;
using Client.Services.Receiver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAnalyzer();
builder.Services.AddReceiver(builder.Configuration);

var app = builder.Build();
app.Run();