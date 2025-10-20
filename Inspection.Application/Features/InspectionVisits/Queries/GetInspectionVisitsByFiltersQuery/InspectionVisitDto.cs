using Inspection.Domain.Enums;

namespace Inspection.Application.Features.InspectionVisits.Queries.GetInspectionVisitsByFiltersQuery;
public class InspectionVisitDto
{
    public Guid Id { get; set; }
    public string EntityName { get; set; }
    public string InspectorName { get; set; }
    public DateTime ScheduledAt { get; set; }
    public VisitStatus Status { get; set; }
    public int Score { get; set; }
    public string Notes { get; set; }
}
