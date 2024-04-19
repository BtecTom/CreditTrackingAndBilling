using DataBaseAccess.Models;

namespace BusinessLogic
{
    public class NotificationService
    {
        private readonly double _warningPercentage = 0.9;

        public async Task SendNotificationIfNeeded(Organisation organisation)
        {
            if (organisation.CreditsUsed >= organisation.Plan.Credits) 
            {
                NotifyOfAllCreditsUsed(organisation.OrganisationId);
            }
            else if (organisation.CreditsUsed >= organisation.Plan.Credits * _warningPercentage)
            {
                NotifyOfNearFullUsage(organisation.OrganisationId);
            }
        }

        private void NotifyOfAllCreditsUsed(Guid id)
        {
            // sends a notification
        }

        private void NotifyOfNearFullUsage(Guid id)
        {
            // sends a notification
        }

    }
}
