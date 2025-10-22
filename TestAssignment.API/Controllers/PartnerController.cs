using Microsoft.AspNetCore.Mvc;
using TestAssignment.Application.Interfaces;

namespace TestAssignment.API.Controllers;

/// <summary>
/// Represents auth API
/// </summary>
[ApiController]
[Route("api.partner")]
public class PartnerController : ControllerBase
{
    private readonly IAuthService _authService;

    public PartnerController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// (Optional) Saves user by unique code and returns auth token required on all other requests, if implemented.
    /// </summary>
    /// <param name="request">Request containing the user code</param>
    /// <returns>Token information</returns>
    [HttpPost("rememberMe")]
    public async Task<IActionResult> RememberMe([FromBody] RememberMeRequest request)
    {
        var tokenInfo = await _authService.RememberMeAsync(request.Code);
        return Ok(tokenInfo);
    }
}

public record RememberMeRequest(string Code);

