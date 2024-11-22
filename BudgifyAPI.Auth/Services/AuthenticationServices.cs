using BudgifyAPI.Auth.Helpers.Interfaces;
using BudgifyAPI.Auth.Models.DB;
using BudgifyAPI.Auth.Models.Queries.Interfaces;
using BudgifyAPI.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BudgifyAPI.Auth.Services;

public class AuthenticationServices:IAuthenticationServices
{
    private readonly IAuthenticationQueries _authenticationQueries;
    private readonly IPasetoHelper _pasetoHelper;

    public AuthenticationServices(IAuthenticationQueries authenticationQueries, IPasetoHelper pasetoHelper)
    {
        _authenticationQueries = authenticationQueries;
        _pasetoHelper = pasetoHelper;
        
    }
    public async Task<string?> login(string email, string password)
    {
        Guid? idUser = await _authenticationQueries.Login(email, password);
        if(idUser!=null){
           return _pasetoHelper.GeneratePasetoToken(idUser.Value);
        }
        return null;
        
    }

    public async Task<bool> register(string email, string password, string name, DateOnly dateOfBirth, int genre)
    {
        return await _authenticationQueries.Register(email, password, name, dateOfBirth, genre);
    }
    public async Task<List<User>> GetUsers()
    {
        return await _authenticationQueries.GetUsers();
    }
}