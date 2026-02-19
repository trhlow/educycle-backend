using EduCycle.Domain.Entities;
using EduCycle.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduCycle.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    // OAuth methods
    public async Task<User?> GetByGoogleIdAsync(string googleId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.GoogleId == googleId);
    }

    public async Task<User?> GetByFacebookIdAsync(string facebookId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.FacebookId == facebookId);
    }

    public async Task<User?> GetByMicrosoftIdAsync(string microsoftId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.MicrosoftId == microsoftId);
    }
}
