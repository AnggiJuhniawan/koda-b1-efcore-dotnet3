using KodaStore.Api.DTOs;

namespace KodaStore.Api.Services;

public interface IOrderService
{
    Task<IReadOnlyList<OrderSummaryResponse>> GetPageAsync(int page, int size, CancellationToken ct);
    Task<OrderDetailResponse?> GetByIdAsync(long id, CancellationToken ct);
    Task<OrderDetailResponse> CreateAsync(CreateOrderRequest request, CancellationToken ct);
    Task<OrderDetailResponse?> ConfirmAsync(long id, CancellationToken ct);
}