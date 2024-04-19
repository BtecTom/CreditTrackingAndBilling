namespace DataBaseAccess.Models
{
    public class TrialUser : User
    {
        public DateTime FirstReportRanDate { get; set; }

        public bool TrialCompleted {  get; set; }
    }
}
