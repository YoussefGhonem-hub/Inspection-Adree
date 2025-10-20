using System.ComponentModel.DataAnnotations.Schema;

namespace Inspection.Domain.Entities;

[Table("ExceptionLog", Schema = "Log")]

public class ExceptionLog : BaseEntity
{
    public string? ExceptionType { get; set; }
    public string? Message { get; set; }
    public string? StackTrace { get; set; }
    public string? Source { get; set; }
    public int? StatusCode { get; set; }
    public string? Path { get; set; }
    public string? QueryString { get; set; }

    public string? InnerExceptionType { get; set; }
    public string? InnerExceptionMessage { get; set; }
    public string? AdditionalData { get; set; }
}
