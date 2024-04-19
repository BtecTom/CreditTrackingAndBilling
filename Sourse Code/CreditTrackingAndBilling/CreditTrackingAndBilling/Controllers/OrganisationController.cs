using BusinessLogic;
using DataBaseAccess;
using Microsoft.AspNetCore.Mvc;

namespace CreditTrackingAndBilling.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrganisationController(ILogger<OrganisationController> logger, CreditTrackingDbContext dbContext, 
        NotificationService notificationService, AccessControlService accessControlService)
        : ControllerBase
    {
        private readonly OrganisationManagement _organisationManagement = new(dbContext, notificationService);

        [HttpPut(Name = "SetPerUserCreditLimit")]
        [ActionName("SetPerUserCreditLimit")]
        public async Task<HttpResponseMessage> SetPerUserCreditLimit(string token, Guid organisationId, uint perUserLimit)
        {
            if (!accessControlService.Validate(token))
            {
                logger.Log(LogLevel.Information, $"Token failed to validate: {token}");
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            var result = await _organisationManagement.SetPerUserCreditLimit(organisationId, perUserLimit);
            logger.Log(LogLevel.Information, $"Set Per User Limit completed with code: {result.StatusCode}");
            return result;
        }
    }
}
