using Inspection.Domain.Enums;

namespace Inspection.Application.Features.Inspectors.Queries.GetAllInspectorsQuery;
public class InspectorDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public Role Role { get; set; }
}
