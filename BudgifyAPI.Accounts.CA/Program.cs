using System.Net;
using BudgifyAPI.Accounts.CA.Controllers;
using BudgifyAPI.Accounts.CA.Entities;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddGrpc();
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any,65084, o=>o.Protocols=HttpProtocols.Http1);
    options.Listen(IPAddress.Any,65085, o => { 
        o.Protocols = HttpProtocols.Http2;
        o.UseHttps("/app/certs/shared.pfx", "budgify");
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapGrpcService<ValidateUserServiceHandler>();
app = AccountsRoute.SetRoutes(app,"/api/v1/Accounts");
app.Run();