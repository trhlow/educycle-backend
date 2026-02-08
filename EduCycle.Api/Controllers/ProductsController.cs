using EduCycle.Api.Common.Extensions;
using EduCycle.Api.Contracts.Products;
using EduCycle.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    // 🔹 CREATE
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        var userId = User.GetUserId();

        var product = new Product
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            CategoryId = request.CategoryId,
            SellerId = userId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return Ok(product);
    }

    // 🔹 GET ALL (PUBLIC)
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .ToListAsync();

        return Ok(products);
    }

    // 🔹 UPDATE (OWNER ONLY)
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductRequest request)
    {
        var userId = User.GetUserId();

        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        if (product.SellerId != userId)
            return Forbid();   // 🔒 OWNER CHECK

        product.Title = request.Title;
        product.Description = request.Description;
        product.Price = request.Price;
        product.CategoryId = request.CategoryId;

        await _context.SaveChangesAsync();
        return Ok(product);
    }

    // 🔹 DELETE (OWNER ONLY)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.GetUserId();

        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        if (product.SellerId != userId)
            return Forbid();   // 🔒 OWNER CHECK

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
