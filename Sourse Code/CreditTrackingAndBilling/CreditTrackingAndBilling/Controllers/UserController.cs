using BusinessLogic;
using DataBaseAccess;
using Microsoft.AspNetCore.Mvc;

namespace CreditTrackingAndBilling.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {

        private readonly TrialUserManagement _trialUserManagement;
        private readonly UserManagement _userManagement;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, CreditTrackingDbContext dbContext, IConfiguration config)
        {
            _logger = logger;
            if (!int.TryParse(config.GetSection("UserTrialSettings")["TrialCredit"], out var trialCredits)||
                !int.TryParse(config.GetSection("UserTrialSettings")["TrialPeriodInDays"], out var trialPeriodInDays))
            {
                throw new InvalidOperationException("TrialCredit or TrialPeriodInDays of invalid type");
            }
            _trialUserManagement = new TrialUserManagement(dbContext, trialCredits, trialPeriodInDays);
            _userManagement = new UserManagement(dbContext);
        }

        [HttpGet(Name = "TrailUserReportRequest")]
        [ActionName("TrailUserReportRequest")]
        public async Task<HttpResponseMessage> TrailUserReportRequest(string token, Guid id, string reportId)
        {
            if (!AccessControlService.Validate(token))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            return await _trialUserManagement.TrailUserReportRequest(id, reportId);
        }

        [HttpGet(Name = "UserReportRequest")]
        [ActionName("UserReportRequest")]
        public async Task<HttpResponseMessage> UserReportRequest(string token, Guid id, string reportId)
        {
            if (!AccessControlService.Validate(token))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            return await _userManagement.UserReportRequest(id, reportId);
        }
    }
}
