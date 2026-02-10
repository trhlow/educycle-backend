using EduCycle.Domain.Entities;
using EduCycle.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduCycle.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext _context;

    public ReviewRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Review review)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Review>> GetAllAsync()
    {
        return await _context.Reviews.AsNoTracking().ToListAsync();
    }
}
