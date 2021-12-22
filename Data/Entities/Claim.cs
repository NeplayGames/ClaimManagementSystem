using ClaimsManagementSystem.Data.Enums;

namespace ClaimsManagementSystem.Data.Entities;

public class Claim
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? OwnerId { get; set; }
    public Status Status { get; set; } = Status.Open;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastStatusChangedAt { get; set; } = DateTimeOffset.UtcNow;
}
