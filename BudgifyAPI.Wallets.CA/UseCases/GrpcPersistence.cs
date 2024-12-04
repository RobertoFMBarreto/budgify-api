using System.Text.Json;
using BudgifyAPI.Wallets.CA.Entities;
using BudgifyAPI.Wallets.CA.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgifyAPI.Wallets.CA.UseCases;

public static class GrpcPersistence
{
    public static async Task<List<string>> GetUserWalletsPersistence(Guid uid)
    {
        WalletsContext context = new WalletsContext();

        try {
            List<Wallet> wallets = await context.Wallets.Where(x => x.IdUser == uid).ToListAsync();
            return wallets.Select(x => x.IdWallet.ToString()).ToList();
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
            return new List<string>();
        }
    }
}