using KodaStore.Api.DTOs;
using KodaStore.Api.Models;
using KodaStore.Api.Repositories;

namespace KodaStore.Api.Services;

public sealed class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    public OrderService(IOrderRepository repository) => _repository = repository;

    public Task<OrderDetailResponse?> ConfirmAsync(long id, CancellationToken ct) => _repository.ConfirmAsync(id, ct);

    public Task<OrderDetailResponse> CreateAsync(CreateOrderRequest request, CancellationToken ct)
    {
        if (request.Items is null || request.Items.Count == 0) throw new ArgumentException("Order minimal 1 item.");

        if (request.Items.Any(i => string.IsNullOrWhiteSpace(i.ProductCode) || i.Quantity <= 0)) throw new ArgumentException("ProductCode dan Quantity harus Valid");

        var codes = request.Items.Select(i => i.ProductCode.Trim().ToUpperInvariant().ToArray());

        // if (codes.Distinct().Count() != codes.Length) throw new ArgumentException("1 ProductCode hanya boleh muncul sekali per Order");

        return _repository.CreateAsync(request, ct);
    }

    public Task<OrderDetailResponse?> GetByIdAsync(long id, CancellationToken ct) => _repository.GetByIdAsync(id, ct);

    public Task<IReadOnlyList<OrderSummaryResponse>> GetPageAsync(int page, int size, CancellationToken ct)
    {
        page = Math.Max(1, page);
        size = Math.Clamp(size, 1, 50);
        if (page > 10_000) throw new ArgumentException("Page terlalu besar untuk pagination");
        return _repository.GetPageAsync(page, size, ct);
    }
}