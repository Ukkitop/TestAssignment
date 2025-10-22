using Microsoft.AspNetCore.Mvc;
using TestAssignment.Application.Interfaces;
using TestAssignment.Application.Models;

namespace TestAssignment.API.Controllers;

/// <summary>
/// Represents journal API
/// </summary>
[ApiController]
[Route("api.user.journal")]
public class JournalController : ControllerBase
{
    private readonly IJournalService _journalService;

    public JournalController(IJournalService journalService)
    {
        _journalService = journalService;
    }

    /// <summary>
    /// Returns the information about an particular event by ID.
    /// </summary>
    /// <param name="request">Request containing the event ID</param>
    /// <returns>Journal entry details</returns>
    [HttpPost("getSingle")]
    public async Task<IActionResult> GetSingle([FromBody] GetSingleRequest request)
    {
        var journal = await _journalService.GetSingleAsync(request.Id);
        if (journal == null)
            return NotFound();

        return Ok(journal);
    }

    /// <summary>
    /// Provides the pagination API. Skip means the number of items should be skipped by server. Take means the maximum number items should be returned by server. All fields of the filter are optional.
    /// </summary>
    /// <param name="request">Request containing skip, take, and optional filter</param>
    /// <returns>Paginated list of journal entries</returns>
    [HttpPost("getRange")]
    public async Task<IActionResult> GetRange([FromBody] GetRangeRequest request)
    {
        var result = await _journalService.GetRangeAsync(
            request.Skip,
            request.Take,
            request.Filter);

        return Ok(result);
    }
}

public record GetSingleRequest(long Id);
public record GetRangeRequest(int Skip, int Take, VJournalFilter? Filter);

