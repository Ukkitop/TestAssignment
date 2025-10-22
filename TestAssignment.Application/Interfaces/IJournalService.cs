using TestAssignment.Application.Models;

namespace TestAssignment.Application.Interfaces;

public interface IJournalService
{
    Task LogExceptionAsync(long eventId, Exception exception, string? queryString, string? requestBody, string? requestPath, string? requestMethod);
    Task<MJournal?> GetSingleAsync(long id);
    Task<MRange<MJournalInfo>> GetRangeAsync(int skip, int take, VJournalFilter? filter);
}

