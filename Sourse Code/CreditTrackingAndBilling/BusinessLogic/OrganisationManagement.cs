using DataBaseAccess;
using DataBaseAccess.Models;

namespace BusinessLogic
{
    public class OrganisationManagement(CreditTrackingDbContext dbContext)
    {
        private readonly NotificationService _notificationService = new();

        public async Task<HttpResponseMessage> SetPerUserLimit(Guid id, uint creditLimitPerUser)
        {
            var organisation = dbContext.Organisations.FirstOrDefault(organisation => organisation.OrganisationId == id);
            if (organisation == null)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }

            organisation.CreditsPerUser = creditLimitPerUser;
            await dbContext.SaveChangesAsync();

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        internal async Task<HttpResponseMessage> OrganisationUserReportRequest(Organisation organisation)
        {

            // If the org did a report was this month, and they're out of credits then fail
            if (ExceededWithTopUp(organisation))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }

            if (organisation.TimeOfLastReportRan?.Month < DateTime.Now.Month || organisation.TimeOfLastReportRan?.Year < DateTime.Now.Year)
            {
                organisation.CreditsUsed = 0;
            }

            organisation.CreditsUsed++;
            organisation.TimeOfLastReportRan = DateTime.Now;
            await dbContext.SaveChangesAsync();

            _ = _notificationService.SendNotificationIfNeeded(organisation);

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        internal bool ExceededWithTopUp(Organisation organisation)
        {
            if (organisation.CreditsUsed >= organisation.Plan.Credits &&
            (organisation.TimeOfLastReportRan?.Month == DateTime.Now.Month && organisation.TimeOfLastReportRan?.Year == DateTime.Now.Year))
            {
                if (organisation.TimeOfLastTopUp?.Month == DateTime.Now.Month && organisation.TimeOfLastTopUp?.Year == DateTime.Now.Year)
                {
                    return organisation.CreditsUsed >= organisation.Plan.Credits + organisation.TopUpCredits;
                }
                return true;
            }
            return false;
        }
    }
}
