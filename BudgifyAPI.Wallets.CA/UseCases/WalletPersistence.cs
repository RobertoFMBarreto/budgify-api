using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgifyAPI.Wallets.CA.Entities;
using BudgifyAPI.Wallets.CA.Framework.EntityFramework.Models;

namespace BudgifyAPI.Wallets.CA.UseCases
{
    public class WalletPersistence
    {
        public async Task<CustomHTTPResponse> RegisterWallet(WalletEntity walletEntity) {
            WalletsContext context = new WalletsContext();
            try
            {

                return new CustomHTTPResponse(200, "wallet created successfully");
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return new CustomHTTPResponse(500, ex.Message);
            }

        }
    }
}