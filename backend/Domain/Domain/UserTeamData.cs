namespace Domain.Domain;

public class UserTeamData {
    public string TeamId { get; set; }
    public string TeamName { get; set; }
    public IList<UserCustomerData> UserCustomersData { get; set; }
}