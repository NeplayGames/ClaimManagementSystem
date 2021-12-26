using ClaimsManagementSystem.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClaimsManagementSystem.Contracts.Claims;

public class ClaimListQueryRequestDto : IValidatableObject
{
    private static readonly string[] AllowedSortBy = new[] { "createdAt", "updatedAt", "title", "status" };
    private static readonly string[] AllowedSortDirection = new[] { "asc", "desc" };

    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 200)]
    public int PageSize { get; set; } = 20;

    public Status? Status { get; set; }

    [StringLength(100)]
    public string? OwnerId { get; set; }

    [StringLength(200)]
    public string? Search { get; set; }

    public string SortBy { get; set; } = "createdAt";

    public string SortDirection { get; set; } = "desc";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!AllowedSortBy.Contains(SortBy, StringComparer.OrdinalIgnoreCase))
        {
            yield return new ValidationResult(
                $"sortBy must be one of: {string.Join(", ", AllowedSortBy)}.",
                new[] { nameof(SortBy) }
            );
        }

        if (!AllowedSortDirection.Contains(SortDirection, StringComparer.OrdinalIgnoreCase))
        {
            yield return new ValidationResult(
                $"sortDirection must be one of: {string.Join(", ", AllowedSortDirection)}.",
                new[] { nameof(SortDirection) }
            );
        }
    }
}