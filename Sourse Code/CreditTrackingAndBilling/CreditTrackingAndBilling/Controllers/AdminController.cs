using BusinessLogic;
using DataBaseAccess;
using Microsoft.AspNetCore.Mvc;

namespace CreditTrackingAndBilling.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AdminController(ILogger<AdminController> logger, CreditTrackingDbContext dbContext, AccessControlService accessControlService)
        : ControllerBase
    {
        private readonly AdminManagement _adminManagement = new(dbContext);

        [HttpPut(Name = "TopUpCustomerAccount")]
        [ActionName("TopUpCustomerAccount")]
        public async Task<HttpResponseMessage> TopUpCustomerAccount(string token, Guid organisationId, uint topUp)
        {
            if (!accessControlService.Validate(token))
            {
                logger.Log(LogLevel.Information, $"Token failed to validate: {token}");
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            var result = await _adminManagement.TopUpCustomerAccount(organisationId, topUp);
            logger.Log(LogLevel.Information, $"Top-Up Customer Account completed with code: {result.StatusCode}");
            return result;
        }
    }
}
