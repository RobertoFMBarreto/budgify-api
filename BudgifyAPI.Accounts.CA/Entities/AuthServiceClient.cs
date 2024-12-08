using System.Text;
using Authservice;
using BudgifyAPI.Accounts.CA.Entities;
using Grpc.Net.Client;

 namespace BudgifyAPI.Accounts.CA.UsesCases;
 
 
 public static class AuthServiceClient
 {
     
     public static async Task<bool> LogoutUser(string uid)
     {
         
         var channel = GrpcChannel.ForAddress(Encoding.UTF8.GetString(Convert.FromBase64String(Environment.GetEnvironmentVariable(
             "grpc__authservice"))));
         AuthService.AuthServiceClient client = new AuthService.AuthServiceClient(channel);
         var request = new LogoutUserRequest() { Uid = Convert.ToBase64String(Encoding.UTF8.GetBytes(CustomEncryptor.EncryptString(uid)))};
         var response = await client.LogoutUserAsync(request);
 
         return response.Done;
     }
 }