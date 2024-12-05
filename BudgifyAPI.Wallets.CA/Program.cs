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
    options.ListenLocalhost(7131, o=>o.Protocols=HttpProtocols.Http1);
    options.ListenLocalhost(7132, o => { 
        o.Protocols = HttpProtocols.Http2;
        o.UseHttps();
    });
});

var app = builder.Build();

app.MapGrpcService<WalletsGrpcService>();
WalletRoute.setRoutes(app,"/api/v1/wallets");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

