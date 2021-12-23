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

    public async Task<PagedResponseDto<ClaimListItemResponseDto>> GetClaimsAsync(ClaimListQueryRequestDto query, CancellationToken cancellationToken = default)
    {
        var claimsQuery = _dbContext.Claims.AsNoTracking();

        if (query.Status.HasValue)
        {
            claimsQuery = claimsQuery.Where(c => c.Status == query.Status.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.OwnerId))
        {
            var ownerId = query.OwnerId.Trim();
            claimsQuery = claimsQuery.Where(c => c.OwnerId == ownerId);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            claimsQuery = claimsQuery.Where(c => c.Title.Contains(search) || c.Description.Contains(search));
        }

        claimsQuery = ApplySorting(claimsQuery, query.SortBy, query.SortDirection);

        var totalCount = await claimsQuery.CountAsync(cancellationToken);
        var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)query.PageSize);

        var claims = await claimsQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
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

        return new PagedResponseDto<ClaimListItemResponseDto>
        {
            Items = claims,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<ClaimDetailsResponseDto> GetClaimByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var claim = await FindClaimOrThrowAsync(id, cancellationToken);
        return MapToDetailsDto(claim);
    }

    public async Task<ClaimDetailsResponseDto> CreateClaimAsync(CreateClaimRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedTitle = request.Title.Trim();
        var duplicateExists = await _dbContext.Claims.AnyAsync(c => c.Title == normalizedTitle, cancellationToken);
        if (duplicateExists)
        {
            throw new ConflictException($"A claim with title '{normalizedTitle}' already exists.", "duplicate_claim_title");
        }

        var now = DateTimeOffset.UtcNow;

        var claim = new Claim
        {
            Id = Guid.NewGuid(),
            Title = normalizedTitle,
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
        var normalizedTitle = request.Title.Trim();

        var duplicateExists = await _dbContext.Claims
            .AnyAsync(c => c.Id != id && c.Title == normalizedTitle, cancellationToken);

        if (duplicateExists)
        {
            throw new ConflictException($"A claim with title '{normalizedTitle}' already exists.", "duplicate_claim_title");
        }

        var now = DateTimeOffset.UtcNow;

        claim.Title = normalizedTitle;
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

    private static IQueryable<Claim> ApplySorting(IQueryable<Claim> query, string sortBy, string sortDirection)
    {
        var isAscending = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase);

        return sortBy.ToLowerInvariant() switch
        {
            "updatedat" => isAscending ? query.OrderBy(c => c.UpdatedAt) : query.OrderByDescending(c => c.UpdatedAt),
            "title" => isAscending ? query.OrderBy(c => c.Title) : query.OrderByDescending(c => c.Title),
            "status" => isAscending ? query.OrderBy(c => c.Status) : query.OrderByDescending(c => c.Status),
            _ => isAscending ? query.OrderBy(c => c.CreatedAt) : query.OrderByDescending(c => c.CreatedAt)
        };
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
