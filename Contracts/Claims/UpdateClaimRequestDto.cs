using ClaimsManagementSystem.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClaimsManagementSystem.Contracts.Claims;

public class UpdateClaimRequestDto
{
    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? OwnerId { get; set; }

    public Status? Status { get; set; }
}
