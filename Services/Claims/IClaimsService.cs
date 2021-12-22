using ClaimsManagementSystem.Contracts.Claims;

namespace ClaimsManagementSystem.Services.Claims;

public interface IClaimsService
{
    Task<IReadOnlyCollection<ClaimListItemResponseDto>> GetClaimsAsync(CancellationToken cancellationToken = default);
    Task<ClaimDetailsResponseDto> GetClaimByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ClaimDetailsResponseDto> CreateClaimAsync(CreateClaimRequestDto request, CancellationToken cancellationToken = default);
    Task<ClaimDetailsResponseDto> UpdateClaimAsync(Guid id, UpdateClaimRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteClaimAsync(Guid id, CancellationToken cancellationToken = default);
}
