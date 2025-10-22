using System.ComponentModel.DataAnnotations;

namespace TestAssignment.Domain.Entities;

public class ExceptionJournal
{
    public long Id { get; set; }

    public long EventId { get; set; }

    public DateTime CreatedAt { get; set; }

    [Required]
    [MaxLength(500)]
    public string ExceptionType { get; set; } = string.Empty;

    [Required]
    public string ExceptionMessage { get; set; } = string.Empty;

    [Required]
    public string StackTrace { get; set; } = string.Empty;

    public string? QueryString { get; set; }

    public string? RequestBody { get; set; }

    public string? RequestPath { get; set; }

    public string? RequestMethod { get; set; }
}

