namespace ClaimsManagementSystem.Contracts.Claims;

public class ClaimDetailsResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
}
