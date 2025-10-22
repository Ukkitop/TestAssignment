namespace TestAssignment.Application.Models;

public class MNode
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<MNode> Children { get; set; } = new();
}

