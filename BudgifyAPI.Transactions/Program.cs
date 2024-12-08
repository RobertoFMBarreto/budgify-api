using System.Net;
using System.Text.Json.Serialization;
using BudgifyAPI.Transactions.Controller;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any,65086, o=>o.Protocols=HttpProtocols.Http1);
    options.Listen(IPAddress.Any,65087, o => { 
        o.Protocols = HttpProtocols.Http2;
        o.UseHttps("/app/certs/shared.pfx", "budgify");
    });
});

var app = builder.Build();

TransactionsRoute.SetRoutes(app,"/api/v1/transactions");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
