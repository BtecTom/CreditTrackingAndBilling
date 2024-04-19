using DataBaseAccess;
using DataBaseAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic
{
    public class UserManagement(CreditTrackingDbContext dbContext)
    {
        public async Task<HttpResponseMessage> UserReportRequest(Guid id, string reportId)
        {
            var user = dbContext.Users.Include(u => u.Organisation).Include(u => u.Organisation.Plan ).FirstOrDefault(user => user.UserId == id);

            if (user == null)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }

            // If the user did a report was this month, and they're out of credits then fail
            if (user.CreditsUsed >= user.Organisation.CreditsPerUser && 
                (user.TimeOfLastReportRan?.Month == DateTime.Now.Month && user.TimeOfLastReportRan?.Year == DateTime.Now.Year))
            {
                dbContext.ReportRequests.Add(new ReportRequest { User = user, ReportId = reportId, ReportRequestSuccessful = false});
                await dbContext.SaveChangesAsync();
                return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }

            if (user.TimeOfLastReportRan?.Month < DateTime.Now.Month || user.TimeOfLastReportRan?.Year < DateTime.Now.Year)
            {
                user.CreditsUsed = 0;
            }

            // now check the org
            var orgResult = await new OrganisationManagement(dbContext).OrganisationUserReportRequest(user.Organisation);
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
}
