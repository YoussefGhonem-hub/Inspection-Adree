using System.ComponentModel.DataAnnotations;

namespace Inspection.Domain.Entities;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public DateTimeOffset CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTimeOffset? ModifiedDate { get; set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}