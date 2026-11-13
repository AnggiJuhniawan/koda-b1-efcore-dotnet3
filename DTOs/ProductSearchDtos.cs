using System.ComponentModel.DataAnnotations;

namespace KodaStore.Api.DTOs;

public sealed class ProductSearchRequst : IValidatableObject
{
    [StringLength(60)]
    public string? Search { get; set; }

    public string? Category { get; set; }

    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    [Required, StringLength(20)]
    public string Sort { get; set; } = "code_asc";

    [Range(1, 1000)]
    public int Page { get; set; } = 1;
    [Range(1, 50)]
    public int PageSize { get; set; } = 10;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MinPrice < 0)
            yield return new ValidationResult("minPrice tidak boleh negatif.", [nameof(MinPrice)]);

        if (MaxPrice < 0)
            yield return new ValidationResult("MaxPrice tidak boleh negatif", [nameof(MaxPrice)]);

        if (MinPrice.HasValue && MaxPrice.HasValue && MinPrice > MaxPrice)
        {
            yield return new ValidationResult("MinPrice tidak boleh melebihi MaxPrice", [nameof(MinPrice), nameof(MaxPrice)]);
        }

        string[] allowedSort = [
            "code_asc", "code_desc",
            "name_asc", "name_desc",
            "price_asc", "price_desc",
            "stock_asc", "stock_desc"
        ];
        if (!allowedSort.Contains(Sort, StringComparer.OrdinalIgnoreCase))
        {
            yield return new ValidationResult("Sort nya ga ada bang di whitelist", [nameof(Sort)]);
        }
    }
}

public sealed record ProductSearchItemResponse(
    string ProductCode,
    string Name,
    string Category,
    decimal Price,
    int Stock
);

public sealed record PagedResult<T>(
    int Page, int PageSize, long TotalCount,
    long TotalPages, IReadOnlyList<T> Items
);