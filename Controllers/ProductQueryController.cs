using KodaStore.Api.DTOs;
using KodaStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KodaStore.Api.Controllers;

[ApiController]
[Route("api/v1/products")]
public sealed class ProductQueryController(IProductQueryService service) : ControllerBase
{
    private readonly IProductQueryService _service = service;

    [HttpGet("search")]

    [ProducesResponseType(typeof(PagedResult<ProductSearchItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PagedResult<ProductSearchItemResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(PagedResult<ProductSearchItemResponse>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<ProductSearchItemResponse>>> Search(
        [FromQuery] ProductSearchRequst request, CancellationToken ct
    )
    {
        var response = await _service.SearchAsync(
            request, ct
        );

        return Ok(response);
    }
}