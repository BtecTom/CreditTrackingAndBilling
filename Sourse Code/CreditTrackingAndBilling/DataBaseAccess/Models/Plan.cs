namespace DataBaseAccess.Models
{
    public class Plan
    {
        public Guid PlanId { get; set; }

        public required string PlanName { get; set; }

        public uint Credits {  get; set; }
    }
}
