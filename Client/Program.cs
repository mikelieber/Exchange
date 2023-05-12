using Client.DAL;
using Client.Data;
using Client.Services.Analyzer;
using Client.Services.Receiver;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<QuotesContext>(o => o.UseInMemoryDatabase("quotes_db"));
builder.Services.AddScoped<UnitOfWork>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAnalyzer();
builder.Services.AddReceiver(builder.Configuration);


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();