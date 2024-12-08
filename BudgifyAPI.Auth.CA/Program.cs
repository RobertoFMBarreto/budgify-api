using System.Net;
using System.Text;
using Authservice;
using BudgifyAPI.Auth.CA.controllers;
using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Auth.CA.UseCases;
using Grpc.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Validateuserservice;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any,65082, o=>o.Protocols=HttpProtocols.Http1);
    options.Listen(IPAddress.Any,65083, o => { 
        o.Protocols = HttpProtocols.Http2;
        o.UseHttps("/app/certs/shared.pfx", "budgify");
    });
});

var app = builder.Build();

app.MapGrpcService<AuthServiceHandler>();

app = UserRoute.SetRoutes(app,"/api/v1/authentication");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
