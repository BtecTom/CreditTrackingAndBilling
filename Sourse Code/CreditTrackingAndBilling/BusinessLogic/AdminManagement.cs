using DataBaseAccess;

namespace BusinessLogic
{
    public class AdminManagement(CreditTrackingDbContext dbContext)
    {
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
}
