using Inspection.Domain.Enums;

namespace Inspection.Domain.Entities;
public class Violation : BaseEntity
{
    public Guid InspectionVisitId { get; set; }
    public InspectionVisit InspectionVisit { get; set; }

    public string Code { get; set; }
    public string Description { get; set; }
    public ViolationSeverity Severity { get; set; }
}