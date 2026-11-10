using System.ComponentModel.DataAnnotations;

namespace KodaStore.Api.DTOs;

public sealed record CreateOrderRequest(
    [Required, StringLength(35, MinimumLength = 3)] string OrderNumber,
    [Required, MinLength(1)] List<CreateOrderItemRequest> Items);

public sealed record CreateOrderItemRequest(
    [Required, StringLength(35)] string ProductCode,
    [Range(1, 1000)] int Quantity);

public sealed record OrderSummaryResponse(
    long OrderId, string OrderNumber, DateTimeOffset OrderDate, string Status, decimal TotalAmount);

public sealed record OrderItemResponse(
    string ProductCode, string ProductName, int Quantity, decimal UnitPrice, decimal Subtotal);

public sealed record OrderDetailResponse(
    long OrderId, string OrderNumber, DateTimeOffset OrderDate, string Status, decimal TotalAmount, IReadOnlyList<OrderItemResponse> Items);