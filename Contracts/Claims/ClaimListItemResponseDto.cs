namespace ClaimsManagementSystem.Contracts.Claims;

public class ClaimListItemResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}
