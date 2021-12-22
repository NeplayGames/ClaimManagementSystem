using ClaimsManagementSystem.Contracts.Claims;
using ClaimsManagementSystem.Services.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly IClaimsService _claimsService;

    public ClaimsController(IClaimsService claimsService)
    {
        _claimsService = claimsService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ClaimListItemResponseDto>>> GetClaims(CancellationToken cancellationToken)
    {
        var claims = await _claimsService.GetClaimsAsync(cancellationToken);
        return Ok(claims);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ClaimDetailsResponseDto>> GetClaimById(Guid id, CancellationToken cancellationToken)
    {
        var claim = await _claimsService.GetClaimByIdAsync(id, cancellationToken);
        return Ok(claim);
    }

    [HttpPost]
    public async Task<ActionResult<ClaimDetailsResponseDto>> CreateClaim([FromBody] CreateClaimRequestDto request, CancellationToken cancellationToken)
    {
        var claim = await _claimsService.CreateClaimAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetClaimById), new { id = claim.Id }, claim);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ClaimDetailsResponseDto>> UpdateClaim(Guid id, [FromBody] UpdateClaimRequestDto request, CancellationToken cancellationToken)
    {
        var claim = await _claimsService.UpdateClaimAsync(id, request, cancellationToken);
        return Ok(claim);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteClaim(Guid id, CancellationToken cancellationToken)
    {
        await _claimsService.DeleteClaimAsync(id, cancellationToken);
        return NoContent();
    }
}
