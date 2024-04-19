using DataBaseAccess;
using DataBaseAccess.Models;

namespace BusinessLogic
{
    public sealed class OrganisationManagement(CreditTrackingDbContext dbContext, NotificationService notificationService)
    {
        /// <summary>
        /// Sets the per use credit limit on the organisation
        /// </summary>
        /// <param name="id">The Organisations Id</param>
        /// <param name="creditLimitPerUser">The limit new credit limit per user</param>
        /// <returns>A task with a HttpResponseMessage once completed</returns>
        public async Task<HttpResponseMessage> SetPerUserCreditLimit(Guid id, uint creditLimitPerUser)
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

        /// <summary>
        /// Validates that an organisation has enough credits to make a request 
        /// </summary>
        /// <param name="organisation">The organisation</param>
        /// <returns>A task with a HttpResponseMessage once completed</returns>
        internal async Task<HttpResponseMessage> OrganisationUserReportRequest(Organisation organisation)
        {
            // If the org did a report was this month, and they're out of credits then fail
            if (OrganisationHasExceededCreditLimit(organisation))
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

            await notificationService.SendNotificationIfNeeded(organisation);

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        private bool OrganisationHasExceededCreditLimit(Organisation organisation)
        {
            if (organisation.TimeOfLastReportRan?.Month == DateTime.Now.Month && organisation.TimeOfLastReportRan?.Year == DateTime.Now.Year &&
                organisation.CreditsUsed >= organisation.Plan.Credits)
            {
                var exceededWithTopUp = ExceededWithTopUp(organisation);
                return exceededWithTopUp == null || exceededWithTopUp.Value;
            }

            return false;
        }

        private bool? ExceededWithTopUp(Organisation organisation)
        {
            // have they had a top-up this month?
            if (organisation.TimeOfLastTopUp?.Month == DateTime.Now.Month && organisation.TimeOfLastTopUp?.Year == DateTime.Now.Year)
            {
                // if they have, then see if they have used them all
                return organisation.CreditsUsed >= organisation.Plan.Credits + organisation.TopUpCredits;
            }
            // else they have no top up so return null, so we know we didn't check credits
            return null;
        }
    }
}
