namespace Core.Domain;

public class CustomerChore
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; }
    public Customer Customer { get; set; }
    public string ChoreId { get; set; }
    public Chore Chore { get; set; }
    public int Frequency { get; set; }
    public int Progress { get; set; }
    public string Status { get; set; }
    public int DaysUntilReset { get; set; }
    public string PeriodicId { get; set; }
    public Periodic Periodic { get; set; }
}