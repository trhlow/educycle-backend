namespace EduCycle.Contracts.Admin;

public class DashboardStatsResponse
{
    public int TotalUsers { get; set; }
    public int TotalProducts { get; set; }
    public int PendingProducts { get; set; }
    public int TotalTransactions { get; set; }
    public decimal TotalRevenue { get; set; }
}
