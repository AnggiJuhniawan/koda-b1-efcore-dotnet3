using KodaStore.Api.DTOs;
using KodaStore.Api.Repositories;

namespace KodaStore.Api.Services;

public sealed class ProductQueryService(IProductQueryRepository repository) : IProductQueryService
{
    private readonly IProductQueryRepository _repository = repository;

    public Task<PagedResult<ProductSearchItemResponse>> SearchAsync(ProductSearchRequst request, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Page is < 1 or > 1000 || request.PageSize is < 1 or > 50)
        {
            throw new ArgumentOutOfRangeException(nameof(request), "Pagination tidak valid");
        }

        if (request.MinPrice < 0 || request.MaxPrice < 0 || (request.MinPrice.HasValue && request.MaxPrice.HasValue && request.MinPrice > request.MaxPrice))
        {
            throw new ArgumentException("Rentang harga tidak valid", nameof(request));
        }

        string? search = string.IsNullOrWhiteSpace(request.Search) ? null : request.Search.Trim();

        string? category = string.IsNullOrWhiteSpace(request.Category) ? null : request.Category.Trim();

        if (search?.Length > 60 || category?.Length > 30)
        {
            throw new ArgumentException("Kepanjangan", nameof(request));
        }

        ProductSort sort = request.Sort?.Trim().ToLowerInvariant() switch
        {
            "code_asc" => ProductSort.CodeAsc,
            "code_desc" => ProductSort.CodeDesc,
            "name_asc" => ProductSort.NameAsc,
            "name_desc" => ProductSort.NameDesc,
            "price_asc" => ProductSort.PriceAsc,
            "price_desc" => ProductSort.PriceDesc,
            "stock_asc" => ProductSort.StockAsc,
            "stock_desc" => ProductSort.StockDesc,

            _ => throw new ArgumentException("Sort tidak dikenali", nameof(request))
        };

        var criteria = new ProductSearchCriteria(
            search, category, request.MinPrice,
            request.MaxPrice, sort,
            request.Page,
            request.PageSize
        );

        return _repository.SearchAsync(criteria, ct);
    }
}