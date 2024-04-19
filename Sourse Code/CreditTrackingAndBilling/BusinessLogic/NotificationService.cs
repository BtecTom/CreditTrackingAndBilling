using DataBaseAccess.Models;

namespace BusinessLogic;

public class NotificationService(double warningPercentage)
{
    /// <summary>
    /// Sets a notification to Xapien if needed
    /// </summary>
    /// <param name="organisation">The organisations details</param>
    /// <returns>A task for sending the notification</returns>
    public virtual Task SendNotificationIfNeeded(Organisation organisation)
    {
        if (organisation.CreditsUsed >= organisation.Plan.Credits) 
        {
            NotifyOfAllCreditsUsed(organisation.OrganisationId);
        }
        else if (organisation.CreditsUsed >= organisation.Plan.Credits * warningPercentage)
        {
            NotifyOfNearFullUsage(organisation.OrganisationId);
        }

        return Task.CompletedTask;
    }

    private static void NotifyOfAllCreditsUsed(Guid id)
    {
        // sends a notification
    }

    private static void NotifyOfNearFullUsage(Guid id)
    {
        // sends a notification
    }

}