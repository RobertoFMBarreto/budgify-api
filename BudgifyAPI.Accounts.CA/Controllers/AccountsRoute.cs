using BudgifyAPI.Accounts.CA.Entities;
using BudgifyAPI.Accounts.CA.UsesCases;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BudgifyAPI.Accounts.CA.Controllers
{
    public static class AccountsRoute
    {
        public static WebApplication SetRoutes(WebApplication application, string baseRoute)
        {
            application.MapGet($"{baseRoute}/user-group/{{id}}", async (Guid userGroupId)=> {
                try
                {
                    CustomHttpResponse resp = await AccountsInteractorEF.GetGroupsById(AccountsPersistence.GetGroupsByIdPersistence, userGroupId);
                    return resp;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            });
            return application;
        }
    }
}
