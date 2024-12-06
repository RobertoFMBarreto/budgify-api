using BudgifyAPI.Transactions.Entities;
using BudgifyAPI.Wallets.CA.Entities;
using BudgifyAPI.Wallets.CA.Entities.Requests;
using BudgifyAPI.Wallets.CA.Entities.Responses.GoCardless;

namespace BudgifyAPI.Transactions.UseCases;

public class GocardlessInteractor
{
    public static async Task<AccessResponse> GetAccessTokenInteractor(Func<Task<AccessResponse>> getAccessTokenPersistence)
    {
        
       return await getAccessTokenPersistence();
    }
    
    public static async Task<CustomHttpResponse> GetBanksInteractor(Func<string,Task<CustomHttpResponse>> getBanksPersistence, string country)
    {
        
        return await getBanksPersistence(country);
    }
    
    public static async Task<CustomHttpResponse> CreateAgreementInteractor(Func<CreateAgreement,Task<CustomHttpResponse>> createAgreementPersistence, CreateAgreement createAgreement)
    {
        return await createAgreementPersistence(createAgreement);
    }
    
    public static async Task<CustomHttpResponse> CreateRequisitionnteractor(Func<CreateRequisitionRequest, Guid,Task<CustomHttpResponse>> createAgreementPersistence, CreateRequisitionRequest requisitionRequest, Guid userId)
    {
        return await createAgreementPersistence(requisitionRequest,userId);
    }
    public static async Task<CustomHttpResponse> GetBankDetailsRequisitionInteractor(Func<string,Task<CustomHttpResponse>> getBankDetailsRequisitionPersistence, string idRequisition)
    {
        return await getBankDetailsRequisitionPersistence(idRequisition);
    }
    public static async Task<CustomHttpResponse> GetTransactionsInteractor(Func<string,Task<CustomHttpResponse>> getTransactionsPersistence, string idAccount)
    {
        return await getTransactionsPersistence(idAccount);
    }
}