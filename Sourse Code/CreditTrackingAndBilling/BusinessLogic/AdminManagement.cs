using DataBaseAccess;

namespace BusinessLogic;

public sealed class AdminManagement(CreditTrackingDbContext dbContext)
{
    /// <summary>
    /// Adds a top-up the customers 
    /// </summary>
    /// <param name="id">The customers Id</param>
    /// <param name="topUp">The amount to add to the customers top-up</param>
    /// <returns>HttpResponseMessage</returns>
    public async Task<HttpResponseMessage> TopUpCustomerAccount(Guid id, uint topUp)
    {
        var organisation = dbContext.Organisations.FirstOrDefault(organisation => organisation.OrganisationId == id);
        if (organisation == null)
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
        }

        organisation.TopUpCredits += topUp;
        organisation.TimeOfLastTopUp = DateTime.Now;
        await dbContext.SaveChangesAsync();

        return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
    }
}