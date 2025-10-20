namespace Inspection.Application.Features.InspectionVisits.Queries.DashboardQuery;
public class DashboardDto
{
    public int PlannedCount { get; set; }
    public int InProgressCount { get; set; }
    public int CompletedCount { get; set; }
    public int CancelledCount { get; set; }
    public double AverageScore { get; set; }
}