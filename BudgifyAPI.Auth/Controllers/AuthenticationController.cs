using BudgifyAPI.Auth.Models.DB;
using BudgifyAPI.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = BudgifyAPI.Auth.Models.Request.LoginRequest;
using RegisterRequest = BudgifyAPI.Auth.Models.Request.RegisterRequest;

namespace BudgifyAPI.Auth.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationServices _authenticationServices;
    public AuthenticationController(IAuthenticationServices authenticationServices)
    {
        _authenticationServices = authenticationServices;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromBody] LoginRequest request)
    { 
        string result = await _authenticationServices.login(request.Email,request.Password);
        return result!=null? Ok(result): Unauthorized();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Get([FromBody] RegisterRequest request)
    { 
        bool result = await _authenticationServices.register(request.Email,request.Pwd,request.Name,request.DateOfBirth,request.Genre);
        return result? Ok(): Unauthorized();
    }
    
    [HttpGet("users")]
    public async Task<IActionResult> Get()
    {
        List<User> users = await _authenticationServices.GetUsers();
        return Ok(users);
    }
}