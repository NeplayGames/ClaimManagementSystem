using Microsoft.AspNetCore.Mvc;

namespace ClaimsManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetClaims()
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetClaimById(Guid id)
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [HttpPost]
    public IActionResult CreateClaim()
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateClaim(Guid id)
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteClaim(Guid id)
    {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }
}
