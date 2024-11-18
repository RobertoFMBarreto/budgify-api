using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace BudgifyAPI.Wallets
{
    [ApiController]
    [Route("[controller]")]
    public class WalletController : ControllerBase
    {
    //    private readonly ILogger<WalletController> _logger;

    //    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    //    {
    //        _logger = logger;
    //    }

        [HttpPost(Name = "CreateWallet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<bool> CreateWallet()
        {
            return true;
        }
    }

}
