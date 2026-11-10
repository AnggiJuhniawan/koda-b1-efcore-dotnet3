using KodaStore.Api.DTOs;
using KodaStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KodaStore.Api.Controllers;

[ApiController]
[Route("api/v1/orders")]
public sealed class OrderController : ControllerBase
{
    private readonly IOrderService _service;
    public OrderController(IOrderService service) => _service = service;

    // GET
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderSummaryResponse>>> GetPage(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        try
        {
            return Ok(await _service.GetPageAsync(page, pageSize, ct));
        }
        catch (ArgumentException e)
        {
            return BadRequest(new
            ProblemDetails
            {
                Title = "Invalid Pagination",
                Detail = e.Message,
                Status = 400
            });
        }
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<OrderDetailResponse>> GetById(
        [FromRoute] long id, CancellationToken ct
    )
    {
        var order = await _service.GetByIdAsync(id, ct);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDetailResponse>> Create(
        [FromBody] CreateOrderRequest request, CancellationToken ct
    )
    {
        try
        {
            var created = await _service.CreateAsync(request, ct);

            return CreatedAtAction(nameof(GetById), new { id = created.OrderId }, created);
        }
        catch (ArgumentException e)
        {
            return BadRequest(new
            ProblemDetails
            {
                Title = "Invalid Order",
                Detail = e.Message,
                Status = 400
            });
        }
    }

    [HttpPatch("{id:long}/confirm")]
    public async Task<ActionResult<OrderDetailResponse>> Confrim([FromRoute] long id, CancellationToken ct)
    {
        try
        {
            var result = await _service.ConfirmAsync(id, ct);
            return result is null ? NotFound() : Ok(result);
        }
        catch (InvalidOperationException e)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Invalid Order State",
                Detail = e.Message,
                Status = 400
            });
        }
    }
}