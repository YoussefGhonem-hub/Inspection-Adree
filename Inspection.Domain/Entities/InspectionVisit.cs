using Inspection.Domain.Enums;

namespace Inspection.Domain.Entities;
public class InspectionVisit : BaseEntity
{
    public Guid EntityToInspectId { get; set; }
    public EntityToInspect EntityToInspect { get; set; }

    public Guid InspectorId { get; set; }
    public Inspector Inspector { get; set; }

    public DateTime ScheduledAt { get; set; }
    public VisitStatus Status { get; set; }
    public int Score { get; set; }
    public string Notes { get; set; }

    public ICollection<Violation> Violations { get; set; } = new List<Violation>();
}