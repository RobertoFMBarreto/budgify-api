using System.Text;
using Authservice;
 using Grpc.Net.Client;
 using Paseto;

 namespace BudgifyAPI.Gateway.Entities;
 
 
 public static class AuthServiceClient
 {
     
     public static async Task<bool> ValidateRefreshTokenAsync(string token, string userAgent)
     {
         var channel = GrpcChannel.ForAddress("https://localhost:5067");
         AuthService.AuthServiceClient _client = new AuthService.AuthServiceClient(channel);
         var request = new ValidateTokenRequest { Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(CustomEncryptor.EncryptString(token))) , Agent = userAgent };
         var response = await _client.ValidateRefreshTokenAsync(request);
 
         return response.IsValid;
     }
 }