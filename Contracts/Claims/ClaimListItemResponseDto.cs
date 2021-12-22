using ClaimsManagementSystem.Data.Enums;

namespace ClaimsManagementSystem.Contracts.Claims;

public class ClaimListItemResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? OwnerId { get; set; }
    public Status Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
