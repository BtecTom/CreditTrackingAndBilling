namespace DataBaseAccess.Models
{
    public class OrganisationalUser: User
    {
        public required Organisation Organisation { get; set; }
    }
}
