using DataBaseAccess;
using DataBaseAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic;

public sealed class OrganisationalUserManagement(CreditTrackingDbContext dbContext, NotificationService notificationService)
{
    /// <summary>
    /// Request a report for an organisational user
    /// </summary>
    /// <param name="id">The organisational users Id</param>
    /// <param name="reportId">The Report Id being requested</param>
    /// <returns>A task with a HttpResponseMessage once completed</returns>
    public async Task<HttpResponseMessage> UserReportRequest(Guid id, string reportId)
    {
        var user = dbContext.Users.Include(u => u.Organisation).Include(u => u.Organisation.Plan ).FirstOrDefault(user => user.UserId == id);

        if (user == null)
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
        }

        // If the User hasn't done a report this month, reset their credits used
        if (user.TimeOfLastReportRan?.Month != DateTime.Now.Month && user.TimeOfLastReportRan?.Year != DateTime.Now.Year)
        {
            user.CreditsUsed = 0;
        }
        // If they have done one this month check if they have exceeded their per-user credit limit
        else if (user.CreditsUsed >= user.Organisation.CreditsPerUser)
        {
            dbContext.ReportRequests.Add(new ReportRequest { User = user, ReportId = reportId, ReportRequestSuccessful = false});
            await dbContext.SaveChangesAsync();
            return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
        }

        // now check the org
        var orgResult = await new OrganisationManagement(dbContext, notificationService).OrganisationUserReportRequest(user.Organisation);
        if (orgResult.StatusCode != System.Net.HttpStatusCode.OK)
        {
            dbContext.ReportRequests.Add(new ReportRequest { User = user, ReportId = reportId, ReportRequestSuccessful = false });
            await dbContext.SaveChangesAsync();
            return orgResult;
        }

        user.CreditsUsed++;
        user.TimeOfLastReportRan = DateTime.Now;

        dbContext.ReportRequests.Add(new ReportRequest { User = user, ReportId = reportId, ReportRequestSuccessful = true});

        await dbContext.SaveChangesAsync();
        return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
    }
}