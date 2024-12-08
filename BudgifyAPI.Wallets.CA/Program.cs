using System.Net;
using BudgifyAPI.Wallets.CA.Controllers;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpc();
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any,65088, o=>o.Protocols=HttpProtocols.Http1);
    options.Listen(IPAddress.Any,65089, o => { 
        o.Protocols = HttpProtocols.Http2;
        o.UseHttps("/app/certs/shared.pfx", "budgify");
    });
});

var app = builder.Build();

app.MapGrpcService<WalletsGrpcService>();
WalletRoute.SetRoutes(app,"/api/v1/wallets");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

