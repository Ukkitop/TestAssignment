namespace TestAssignment.Application.Models;

public class ErrorResponse
{
    public long Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public object Data { get; set; } = new();
}

