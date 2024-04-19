namespace DataBaseAccess.Models
{
    public class Organisation
    {
        public Guid OrganisationId { get; set; }

        public required Plan Plan { get; set; }

        public uint CreditsPerUser { get; set; }

        public uint CreditsUsed { get; set; }

        public  DateTime? TimeOfLastReportRan { get; set; }

        public uint TopUpCredits { get; set; }

        public DateTime? TimeOfLastTopUp {  get; set; }
    }
}
