using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BudgifyAPI.Wallets.CA.Entities;
using BudgifyAPI.Wallets.CA.Framework.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgifyAPI.Wallets.CA.UseCases
{
    public static class WalletPersistence
    {
        public static async Task<CustomHTTPResponse> RegisterWallet(WalletEntity walletEntity) {
            WalletsContext context = new WalletsContext();
            try
            {
                await walletEntity.Validate();

                Wallet? wallet = await context.Wallets.Where(x => x.Name == walletEntity.WalletName).FirstOrDefaultAsync();
                if (wallet != null) {
                    return new CustomHTTPResponse(400, "Wallet with same name already exists!");
                }
                else {
                    await context.Wallets.AddAsync(new Wallet() {
                        Name = walletEntity.WalletName!,
                        
                    });
                    await context.SaveChangesAsync();
                }

                return new CustomHTTPResponse(200, "wallet created successfully");
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return new CustomHTTPResponse(500, ex.Message);
            }

        }
        public static async Task<CustomHTTPResponse> DeleteWallet(WalletEntity walletEntity) {
            WalletsContext context = new WalletsContext();

            try {
                Wallet? wallet = await context.Wallets.Where(x => x.IdWallet == walletEntity.WalletId).FirstOrDefaultAsync();
                if (wallet == null) return new CustomHTTPResponse(400, "Wallet not found");
                else {
                    context.Wallets.Remove(wallet);
                    await context.SaveChangesAsync();
                    return new CustomHTTPResponse(200, "Wallet deleted successfully");
                }
            }

            catch (Exception ex) {
                Console.WriteLine(ex);
                return new CustomHTTPResponse(500, ex.Message);
            }
        }

        public static async Task<CustomHTTPResponse> GetWallet(WalletEntity walletEntity) {
            WalletsContext context = new WalletsContext();

            try {
                Wallet? wallet = await context.Wallets.Where(x => x.IdWallet == walletEntity.WalletId).FirstOrDefaultAsync();
                if (wallet != null) {
                    return new CustomHTTPResponse(200, JsonSerializer.Serialize(wallet));
                }
                return new CustomHTTPResponse(400, "Wallet not found");
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                return new CustomHTTPResponse(500, ex.Message);
            }
        }


    }



}