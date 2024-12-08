using System.Net;
using System.Text;
using Authservice;
using BudgifyAPI.Auth.CA.controllers;
using BudgifyAPI.Auth.CA.Entities;
using BudgifyAPI.Auth.CA.UseCases;
using Grpc.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Validateuserservice;


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddAuthentication("Bearer")
//     .AddBearerToken("Bearer", options =>
//     {
//         options.Events = new BearerTokenEvents
//         {
//             OnMessageReceived = context =>
//             {
//                 // Lê o token do cabeçalho Authorization
//                 var authHeader = context.Request.Headers["Authorization"].ToString();
//                 if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
//                 {
//                     context.Token = authHeader.Substring("Bearer ".Length).Trim();
//                 }
//
//                 return Task.CompletedTask;
//             },
//
//         };
//     });
// builder.Services.AddAuthorization(options =>
// {
//     // FallbackPolicy: Permitir acesso público por padrão
//     options.FallbackPolicy = new AuthorizationPolicyBuilder()
//         .RequireAssertion(_ => true) // Permitir acesso a rotas públicas
//         .Build();
//
//     // DefaultPolicy: Continuar protegendo rotas explicitamente com .RequireAuthorization()
//     options.DefaultPolicy = new AuthorizationPolicyBuilder()
//         .AddAuthenticationSchemes("bearer")
//         .RequireAuthenticatedUser()
//         .Build();
// });
//
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Description = @"PASeTo Authorization header using the Bearer scheme. \r\n\r\n 
//                       Enter 'Bearer' [space] and then your token in the text input below.
//                       \r\n\r\nExample: 'Bearer 12345abcdef'",
//         Name = "Authorization",
//         BearerFormat = "PASeTo",
//         In = ParameterLocation.Header,
//         Type = SecuritySchemeType.ApiKey,
//         Scheme = "Bearer"
//     });
//     
//     c.OperationFilter<CustomSecurityFilter>();
//     
//     var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//     var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//     c.IncludeXmlComments(xmlPath);
// });

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
