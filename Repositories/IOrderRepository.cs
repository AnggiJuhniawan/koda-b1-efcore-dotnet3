using KodaStore.Api.DTOs;

namespace KodaStore.Api.Repositories;

public interface IOrderRepository
{
    Task<IReadOnlyList<OrderSummaryResponse>> GetPageAsync(int page, int pageSize, CancellationToken ct);
    Task<OrderDetailResponse?> GetByIdAsync(long id, CancellationToken ct);
    Task<OrderDetailResponse> CreateAsync(CreateOrderRequest request, CancellationToken ct);
    Task<OrderDetailResponse?> ConfirmAsync(long id, CancellationToken ct);
}