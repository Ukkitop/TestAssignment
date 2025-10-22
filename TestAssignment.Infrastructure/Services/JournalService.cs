using Microsoft.EntityFrameworkCore;
using TestAssignment.Application.Interfaces;
using TestAssignment.Application.Models;
using TestAssignment.Domain.Entities;
using TestAssignment.Infrastructure.Data;

namespace TestAssignment.Infrastructure.Services;

public class JournalService : IJournalService
{
    private readonly ApplicationDbContext _context;

    public JournalService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task LogExceptionAsync(long eventId, Exception exception, string? queryString, string? requestBody, string? requestPath, string? requestMethod)
    {
        var journal = new ExceptionJournal
        {
            EventId = eventId,
            CreatedAt = DateTime.UtcNow,
            ExceptionType = exception.GetType().Name,
            ExceptionMessage = exception.Message,
            StackTrace = exception.StackTrace ?? string.Empty,
            QueryString = queryString,
            RequestBody = requestBody,
            RequestPath = requestPath,
            RequestMethod = requestMethod
        };

        _context.ExceptionJournals.Add(journal);
        await _context.SaveChangesAsync();
    }

    public async Task<MJournal?> GetSingleAsync(long id)
    {
        var journal = await _context.ExceptionJournals.FindAsync(id);
        if (journal == null)
            return null;

        return new MJournal
        {
            Id = journal.Id,
            EventId = journal.EventId,
            CreatedAt = journal.CreatedAt,
            Text = FormatJournalText(journal)
        };
    }

    public async Task<MRange<MJournalInfo>> GetRangeAsync(int skip, int take, VJournalFilter? filter)
    {
        var query = _context.ExceptionJournals.AsQueryable();

        // Apply filters
        if (filter != null)
        {
            if (filter.From.HasValue)
            {
                query = query.Where(j => j.CreatedAt >= filter.From.Value);
            }

            if (filter.To.HasValue)
            {
                query = query.Where(j => j.CreatedAt <= filter.To.Value);
            }

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(j =>
                    j.ExceptionType.Contains(filter.Search) ||
                    j.ExceptionMessage.Contains(filter.Search) ||
                    j.StackTrace.Contains(filter.Search));
            }
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip(skip)
            .Take(take)
            .Select(j => new MJournalInfo
            {
                Id = j.Id,
                EventId = j.EventId,
                CreatedAt = j.CreatedAt
            })
            .ToListAsync();

        return new MRange<MJournalInfo>
        {
            Skip = skip,
            Count = totalCount,
            Items = items
        };
    }

    private string FormatJournalText(ExceptionJournal journal)
    {
        return $@"Exception Type: {journal.ExceptionType}
Message: {journal.ExceptionMessage}
Method: {journal.RequestMethod}
Path: {journal.RequestPath}
Query String: {journal.QueryString}
Request Body: {journal.RequestBody}
Stack Trace:
{journal.StackTrace}";
    }
}

