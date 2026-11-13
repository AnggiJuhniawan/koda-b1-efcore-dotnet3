using KodaStore.Api.DTOs;

namespace KodaStore.Api.Services;

public interface IProductQueryService
{
    Task<PagedResult<ProductSearchItemResponse>> SearchAsync(
        ProductSearchRequst request, CancellationToken ct
    );
}