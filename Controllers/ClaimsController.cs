using ClaimsManagementSystem.Contracts.Claims;
using ClaimsManagementSystem.Services.Claims;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Policy = "ClaimsReadAccess")]
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponseDto<ClaimListItemResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PagedResponseDto<ClaimListItemResponseDto>>> GetClaims([FromQuery] ClaimListQueryRequestDto query, CancellationToken cancellationToken)
    {
        var claims = await _claimsService.GetClaimsAsync(query, cancellationToken);
        return Ok(claims);
    }

    [Authorize(Policy = "ClaimsReadAccess")]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ClaimDetailsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClaimDetailsResponseDto>> GetClaimById(Guid id, CancellationToken cancellationToken)
    {
        var claim = await _claimsService.GetClaimByIdAsync(id, cancellationToken);
        return Ok(claim);
    }

    [Authorize(Policy = "ClaimsCreateAccess")]
    [HttpPost]
    [ProducesResponseType(typeof(ClaimDetailsResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClaimDetailsResponseDto>> CreateClaim([FromBody] CreateClaimRequestDto request, CancellationToken cancellationToken)
    {
        var claim = await _claimsService.CreateClaimAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetClaimById), new { id = claim.Id }, claim);
    }

    [Authorize(Policy = "ClaimsUpdateAccess")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ClaimDetailsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ClaimDetailsResponseDto>> UpdateClaim(Guid id, [FromBody] UpdateClaimRequestDto request, CancellationToken cancellationToken)
    {
        var claim = await _claimsService.UpdateClaimAsync(id, request, cancellationToken);
        return Ok(claim);
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteClaim(Guid id, CancellationToken cancellationToken)
    {
        await _claimsService.DeleteClaimAsync(id, cancellationToken);
        return NoContent();
    }
}
