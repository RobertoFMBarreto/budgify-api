using System.Net.Security;
using System.Text;
using BudgifyAPI.Auth.CA.Entities;
using Grpc.Net.Client;
using Validateuserservice;

namespace BudgifyAPI.Auth.CA.UseCases;

public static class ValidateUserClient
{
    public static async Task<Validateuserservice.ValidateUserResponse> ValidateUserAsync(string email, string password)
    {

            var channel = GrpcChannel.ForAddress(Encoding.UTF8.GetString(Convert.FromBase64String(
                Environment.GetEnvironmentVariable(
                    "grpc__accountservice"))));
            ValidateUserService.ValidateUserServiceClient client =
                new ValidateUserService.ValidateUserServiceClient(channel);
            var request = new ValidateUserRequest
            {
                Email = Convert.ToBase64String(Encoding.UTF8.GetBytes(CustomEncryptor.EncryptString(email))),
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(CustomEncryptor.EncryptString(password)))
            };
            var response = await client.ValidateUserServiceAsync(request);
            Console.WriteLine(response.Uid);
            return response;

    }

    public static async Task<Validateuserservice.ValidateUserResponse> GetUserRoleAsync(string uid)
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, chain, sslPolicyErrors) => true
        };
        var channel = GrpcChannel.ForAddress(Encoding.UTF8.GetString(Convert.FromBase64String(Environment.GetEnvironmentVariable(
            "grpc__accountservice"))), new GrpcChannelOptions
        {
            HttpHandler = handler
        });
        ValidateUserService.ValidateUserServiceClient client =
            new ValidateUserService.ValidateUserServiceClient(channel);
        var request = new ValidateUserRequest
        {
            Uid = Convert.ToBase64String(Encoding.UTF8.GetBytes(CustomEncryptor.EncryptString(uid))),
        };
        var response = await client.ValidateUserServiceAsync(request);
        return response;
    }
}