using System.Net;
using BudgifyAPI.Gateway.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Paseto;
using Paseto.Builder;
using Paseto.Cryptography.Key;

var builder = WebApplication.CreateBuilder(args);
PasetoSymmetricKey pasetoKey = new PasetoBuilder().Use(ProtocolVersion.V4, Purpose.Local).GenerateSymmetricKey();
Environment.SetEnvironmentVariable("paseto_key", Convert.ToBase64String(pasetoKey.Key.Span.ToArray()));

File.WriteAllText("./key.txt", Convert.ToBase64String(pasetoKey.Key.Span.ToArray()));
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration).AddDelegatingHandler<AddUserInfoHeaderHandler>().AddDelegatingHandler<AddHostHeaderHandler>();
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any,65080, o=>o.Protocols=HttpProtocols.Http1);
    options.Listen(IPAddress.Any,65081, o => { 
        o.Protocols = HttpProtocols.Http2;
        o.UseHttps("/app/certs/shared.pfx", "budgify");
    });
});

// add authentication
builder.Services.AddAuthentication("Paseto")
    .AddScheme<AuthenticationSchemeOptions,PasetoAuthenticationHandler>("Paseto", null);

var app = builder.Build();

await app.UseOcelot();

app.Run();