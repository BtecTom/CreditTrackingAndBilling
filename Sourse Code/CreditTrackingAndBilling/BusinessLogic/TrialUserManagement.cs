using DataBaseAccess;
using DataBaseAccess.Models;

namespace BusinessLogic;

public sealed class TrialUserManagement(CreditTrackingDbContext dbContext, int trialCredits, int trialPeriodInDays)
{
    /// <summary>
    /// request a report for a trial user
    /// </summary>
    /// <param name="id">The trial users Id</param>
    /// <param name="reportId">The Report Id</param>
    /// <returns>A task with a HttpResponseMessage once completed</returns>
    public async Task<HttpResponseMessage> TrailUserReportRequest(Guid id, string reportId)
    {
        var trialUser = dbContext.TrialUsers.FirstOrDefault(t => t.UserId == id);

        if (trialUser == null)
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
        }

        if (trialUser.CreditsUsed >= trialCredits || trialUser.FirstReportRanDate + new TimeSpan (trialPeriodInDays, 0, 0, 0) < DateTime.Now)
        {
            trialUser.TrialCompleted = true;
            dbContext.ReportRequests.Add(new ReportRequest { User = trialUser, ReportId = reportId, ReportRequestSuccessful = false });
            await dbContext.SaveChangesAsync();
            return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
        }

        trialUser.CreditsUsed++;
        dbContext.ReportRequests.Add(new ReportRequest { User = trialUser, ReportId = reportId, ReportRequestSuccessful = true });
        await dbContext.SaveChangesAsync();
        return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
    }
}