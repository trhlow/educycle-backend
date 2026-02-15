using EduCycle.Contracts.Admin;
using EduCycle.Infrastructure.Data;
using EduCycle.Domain.Enums;
using EduCycle.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduCycle.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IUserRepository _userRepository;

    public AdminController(ApplicationDbContext context, IUserRepository userRepository)
    {
        _context = context;
        _userRepository = userRepository;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetDashboardStats()
    {
        var stats = new DashboardStatsResponse
        {
            TotalUsers = await _context.Users.CountAsync(),
            TotalProducts = await _context.Products.CountAsync(),
            PendingProducts = await _context.Products.CountAsync(p => p.Status == ProductStatus.Pending),
            TotalTransactions = await _context.Transactions.CountAsync(),
            TotalRevenue = await _context.Transactions
                .Where(t => t.Status == TransactionStatus.Completed || t.Status == TransactionStatus.AutoCompleted)
                .SumAsync(t => t.Amount)
        };

        return Ok(stats);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users.Select(u => new
        {
            u.Id,
            u.Username,
            u.Email,
            Role = u.Role.ToString(),
            u.CreatedAt
        }));
    }
}
