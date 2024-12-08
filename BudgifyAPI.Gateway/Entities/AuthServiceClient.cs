using System.Text;
using Authservice;
using BudgifyAPI.Gateway.Entities;
using Grpc.Net.Client;

 namespace BudgifyAPI.Gateway.UseCases;
 
 
 public static class AuthServiceClient
 {
     
     public static async Task<bool> ValidateRefreshTokenAsync(string token, string userAgent)
     {
         var channel = GrpcChannel.ForAddress(Encoding.UTF8.GetString(Convert.FromBase64String(Environment.GetEnvironmentVariable(
             "grpc__authservice"))));
         AuthService.AuthServiceClient client = new AuthService.AuthServiceClient(channel);
         var request = new ValidateTokenRequest { Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(CustomEncryptor.EncryptString(token))) , Agent = userAgent };
         var response = await client.ValidateRefreshTokenAsync(request);
 
         return response.IsValid;
     }
 }