using EduCycle.Api.Common.Extensions;
using EduCycle.Api.Domain.Entities;
using EduCycle.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduCycle.Api.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly EduCycleDbContext _context;

    public ProductsController(EduCycleDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        var userId = User.GetUserId();   // 🔥 USER CONTEXT

        product.SellerId = userId;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return Ok(product);
    }
}
