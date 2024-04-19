using BusinessLogic;
using DataBaseAccess;
using Microsoft.AspNetCore.Mvc;

namespace CreditTrackingAndBilling.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AdminController(ILogger<AdminController> logger, CreditTrackingDbContext dbContext)
        : ControllerBase
    {
        private readonly AdminManagement _adminManagement = new(dbContext);
        private readonly ILogger<AdminController> _logger = logger;

        [HttpPut(Name = "TopUpCustomerAccount")]
        [ActionName("TopUpCustomerAccount")]
        public async Task<HttpResponseMessage> TopUpCustomerAccount(string token, Guid organisationId, uint topUp)
        {
            if (!AccessControlService.Validate(token))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }

            return await _adminManagement.TopUpCustomerAccount(organisationId, topUp);
        }
    }
}
