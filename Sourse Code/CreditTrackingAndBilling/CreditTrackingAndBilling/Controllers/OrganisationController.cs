using BusinessLogic;
using DataBaseAccess;
using Microsoft.AspNetCore.Mvc;

namespace CreditTrackingAndBilling.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrganisationController(ILogger<OrganisationController> logger, CreditTrackingDbContext dbContext)
        : ControllerBase
    {

        private readonly OrganisationManagement _organisationManagement = new(dbContext);
        private readonly ILogger<OrganisationController> _logger = logger;

        [HttpPut(Name = "SetPerUserLimit")]
        [ActionName("SetPerUserLimit")]
        public async Task<HttpResponseMessage> SetPerUserLimit(string token, Guid organisationId, uint perUserLimit)
        {
            if (!AccessControlService.Validate(token))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            return await _organisationManagement.SetPerUserLimit(organisationId, perUserLimit);
        }
    }
}
