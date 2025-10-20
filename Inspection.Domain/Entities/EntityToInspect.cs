namespace Inspection.Domain.Entities;

public class EntityToInspect : BaseEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Category { get; set; }

    public ICollection<InspectionVisit> InspectionVisits { get; set; } = new List<InspectionVisit>();
}
