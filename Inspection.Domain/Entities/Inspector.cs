using Inspection.Domain.Enums;

namespace Inspection.Domain.Entities;
public class Inspector : BaseEntity
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Role Role { get; set; }

    public ICollection<InspectionVisit> InspectionVisits { get; set; } = new List<InspectionVisit>();
}