using ClaimsManagementSystem.Contracts.Claims;
using ClaimsManagementSystem.Data;
using ClaimsManagementSystem.Data.Entities;
using ClaimsManagementSystem.Data.Enums;
using ClaimsManagementSystem.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ClaimsManagementSystem.Services.Claims;

public class ClaimsService : IClaimsService
{
    private readonly ClaimsManagementContext _dbContext;

    public ClaimsService(ClaimsManagementContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<ClaimListItemResponseDto>> GetClaimsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Claims
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new ClaimListItemResponseDto
            {
                Id = c.Id,
                Title = c.Title,
                OwnerId = c.OwnerId,
                Status = c.Status,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ClaimDetailsResponseDto> GetClaimByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var claim = await FindClaimOrThrowAsync(id, cancellationToken);
        return MapToDetailsDto(claim);
    }

    public async Task<ClaimDetailsResponseDto> CreateClaimAsync(CreateClaimRequestDto request, CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;

        var claim = new Claim
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description?.Trim() ?? string.Empty,
            Status = Status.Open,
            CreatedAt = now,
            UpdatedAt = now,
            LastStatusChangedAt = now
        };

        _dbContext.Claims.Add(claim);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToDetailsDto(claim);
    }

    public async Task<ClaimDetailsResponseDto> UpdateClaimAsync(Guid id, UpdateClaimRequestDto request, CancellationToken cancellationToken = default)
    {
        var claim = await FindClaimOrThrowAsync(id, cancellationToken);
        var now = DateTimeOffset.UtcNow;

        claim.Title = request.Title;
        claim.Description = request.Description?.Trim() ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(request.OwnerId))
        {
            claim.OwnerId = request.OwnerId.Trim();
        }

        if (request.Status.HasValue && request.Status.Value != claim.Status)
        {
            EnsureStatusTransitionAllowed(claim.Status, request.Status.Value);
            claim.Status = request.Status.Value;
            claim.LastStatusChangedAt = now;
        }

        claim.UpdatedAt = now;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToDetailsDto(claim);
    }

    public async Task DeleteClaimAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var claim = await FindClaimOrThrowAsync(id, cancellationToken);
        _dbContext.Claims.Remove(claim);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<Claim> FindClaimOrThrowAsync(Guid id, CancellationToken cancellationToken)
    {
        var claim = await _dbContext.Claims.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (claim is null)
        {
            throw new NotFoundException($"Claim '{id}' was not found.");
        }

        return claim;
    }

    private static void EnsureStatusTransitionAllowed(Status current, Status requested)
    {
        var isValid = current switch
        {
            Status.Open => requested is Status.InProgress or Status.Closed,
            Status.InProgress => requested is Status.Closed,
            Status.Closed => false,
            _ => false
        };

        if (!isValid)
        {
            throw new BusinessRuleException($"Invalid status transition from '{current}' to '{requested}'.", "invalid_status_transition");
        }
    }

    private static ClaimDetailsResponseDto MapToDetailsDto(Claim claim)
    {
        return new ClaimDetailsResponseDto
        {
            Id = claim.Id,
            Title = claim.Title,
            Description = claim.Description,
            OwnerId = claim.OwnerId,
            Status = claim.Status,
            CreatedAt = claim.CreatedAt,
            UpdatedAt = claim.UpdatedAt,
            LastStatusChangedAt = claim.LastStatusChangedAt
        };
    }
}
