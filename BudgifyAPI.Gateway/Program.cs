using BudgifyAPI.Gateway.Entities;
using Microsoft.AspNetCore.Authentication;
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
builder.Services.AddOcelot(builder.Configuration).AddDelegatingHandler<AddUserInfoHeaderHandler>();
// add authentication
builder.Services.AddAuthentication("Paseto")
    .AddScheme<AuthenticationSchemeOptions,PasetoAuthenticationHandler>("Paseto", null);

var app = builder.Build();

await app.UseOcelot();

app.Run();