using KodaStore.Api.DTOs;

namespace KodaStore.Api.Repositories;

public interface IProductQueryRepository
{
    Task<PagedResult<ProductSearchItemResponse>> SearchAsync(ProductSearchCriteria criteria, CancellationToken ct);
}