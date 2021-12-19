using ClaimsManagementSystem.Contracts.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ClaimsManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyCollection<ClaimListItemResponseDto>> GetClaims()
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<ClaimDetailsResponseDto> GetClaimById(Guid id)
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [HttpPost]
    public ActionResult<ClaimDetailsResponseDto> CreateClaim([FromBody] CreateClaimRequestDto request)
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [HttpPut("{id:guid}")]
    public ActionResult<ClaimDetailsResponseDto> UpdateClaim(Guid id, [FromBody] UpdateClaimRequestDto request)
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteClaim(Guid id)
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }
}
