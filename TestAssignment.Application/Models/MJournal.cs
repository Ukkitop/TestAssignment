namespace TestAssignment.Application.Models;

public class MJournal
{
    public long Id { get; set; }
    public long EventId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class MJournalInfo
{
    public long Id { get; set; }
    public long EventId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class MRange<T>
{
    public int Skip { get; set; }
    public int Count { get; set; }
    public List<T> Items { get; set; } = new();
}

public class VJournalFilter
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? Search { get; set; }
}

