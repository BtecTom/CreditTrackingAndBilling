using BusinessLogic;
using DataBaseAccess;
using DataBaseAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class OrganisationManagementTests
    {
        [TestMethod]
        public void OrganisationManagement_SetPerUserLimit_ValidInputs()
        {
            var orgId = Guid.NewGuid();

            var optionsBuilder = new DbContextOptionsBuilder<CreditTrackingDbContext>();
            optionsBuilder.UseInMemoryDatabase("Moc Database");
            var dbContext = new CreditTrackingDbContext(optionsBuilder.Options);

            dbContext.Organisations.Add(new Organisation { OrganisationId = orgId, Plan = new Plan { PlanName = "test Plan", Credits = 2, PlanId = Guid.Empty } });
            dbContext.SaveChanges();

            var manager = new OrganisationManagement(dbContext);
            var result = manager.SetPerUserLimit(orgId, 100).Result;

            Assert.IsTrue(result.IsSuccessStatusCode);
            Assert.IsTrue(dbContext.Organisations.First(o => o.OrganisationId == orgId).CreditsPerUser == 100);
        }

        [TestMethod]
        public void OrganisationManagement_SetPerUserLimit_InvalidOrgId()
        {
            var orgId = Guid.NewGuid();

            var optionsBuilder = new DbContextOptionsBuilder<CreditTrackingDbContext>();
            optionsBuilder.UseInMemoryDatabase("Moc Database");
            var dbContext = new CreditTrackingDbContext(optionsBuilder.Options);

            dbContext.Organisations.Add(new Organisation { OrganisationId = orgId, Plan = new Plan { PlanName = "test Plan", Credits = 2, PlanId = Guid.Empty } });
            dbContext.SaveChanges();

            var manager = new OrganisationManagement(dbContext);
            var result = manager.SetPerUserLimit(Guid.NewGuid(), 100).Result;

            Assert.IsTrue(result.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.IsTrue(dbContext.Organisations.First(o => o.OrganisationId == orgId).CreditsPerUser == 0);
        }

        [TestMethod]
        public void OrganisationManagement_SetPerUserLimit_NoOrganisationsPresent()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CreditTrackingDbContext>();
            optionsBuilder.UseInMemoryDatabase("Moc Database");
            var dbContext = new CreditTrackingDbContext(optionsBuilder.Options);

            var manager = new OrganisationManagement(dbContext);
            var result = manager.SetPerUserLimit(Guid.NewGuid(), 100).Result;

            Assert.IsTrue(result.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }
    }
}