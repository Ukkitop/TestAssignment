using System.ComponentModel.DataAnnotations;

namespace TestAssignment.Domain.Entities;

public class TreeNode
{
    public long Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string TreeName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public long? ParentId { get; set; }
    
    public TreeNode? Parent { get; set; }
    
    public List<TreeNode> Children { get; set; } = new();

    public DateTime CreatedAt { get; set; }
}

