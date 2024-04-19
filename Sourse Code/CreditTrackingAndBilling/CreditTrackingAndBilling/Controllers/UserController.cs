using BusinessLogic;
using DataBaseAccess;
using Microsoft.AspNetCore.Mvc;

namespace CreditTrackingAndBilling.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly AccessControlService _accessControlService;
        private readonly TrialUserManagement _trialUserManagement;
        private readonly OrganisationalUserManagement _userManagement;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, CreditTrackingDbContext dbContext, IConfiguration config, 
            NotificationService notificationService, AccessControlService accessControlService)
        {
            _logger = logger;
            if (!int.TryParse(config.GetSection("UserTrialSettings")["TrialCredit"], out var trialCredits)||
                !int.TryParse(config.GetSection("UserTrialSettings")["TrialPeriodInDays"], out var trialPeriodInDays))
            {
                _logger.Log(LogLevel.Critical, "TrialCredit or TrialPeriodInDays of invalid type in Application Configuration");
                throw new InvalidOperationException("TrialCredit or TrialPeriodInDays of invalid type");
            }
            _trialUserManagement = new TrialUserManagement(dbContext, trialCredits, trialPeriodInDays);
            _userManagement = new OrganisationalUserManagement(dbContext, notificationService);
            _accessControlService = accessControlService;
        }

        [HttpGet(Name = "TrailUserReportRequest")]
        [ActionName("TrailUserReportRequest")]
        public async Task<HttpResponseMessage> TrailUserReportRequest(string token, Guid id, string reportId)
        {
            if (!_accessControlService.Validate(token))
            {
                _logger.Log(LogLevel.Information, $"Token failed to validate: {token}");
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            var result = await _trialUserManagement.TrailUserReportRequest(id, reportId);
            _logger.Log(LogLevel.Information, $"Trial User Report Request completed with code: {result.StatusCode}");
            return result;
        }

        [HttpGet(Name = "UserReportRequest")]
        [ActionName("UserReportRequest")]
        public async Task<HttpResponseMessage> UserReportRequest(string token, Guid id, string reportId)
        {
            if (!_accessControlService.Validate(token))
            {
                _logger.Log(LogLevel.Information, $"Token failed to validate: {token}");
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            var result = await _userManagement.UserReportRequest(id, reportId);
            _logger.Log(LogLevel.Information, $"User Report Request completed with code: {result.StatusCode}");
            return result;

        }
    }
}
