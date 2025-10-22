using System.ComponentModel.DataAnnotations;

namespace TestAssignment.Domain.Entities;

public class User
{
    public long Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Code { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime LastLoginAt { get; set; }
}

