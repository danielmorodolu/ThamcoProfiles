using Microsoft.EntityFrameworkCore;
using ProfileService.Models;
using ProfileService.Data;

namespace ProfileService.Services.Profiling
{
public class RealProfileService : IProfileService
{
    private readonly ProfileContext _context;

    public RealProfileService(ProfileContext context)
    {
        _context = context;
    }

    public async Task<Profile?> GetUserByIdAsync(int? id)
    {
        return await _context.Profiles.FindAsync(id);
    }

    public async Task AddUserAsync(Profile user)
    {
        await _context.Profiles.AddAsync(user);
    }

    public async Task<bool> UpdateUser(Profile user)
    {
        var existingUser = await _context.Profiles.FindAsync(user.Id);
        if (existingUser == null) return false;

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.PaymentAddress = user.PaymentAddress;

        return true;
    }

    public bool UserExists(int id)
    {
        return _context.Profiles.Any(p => p.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Profile?> GetUserByAuth0IdAsync(string? id)
    {
        return await _context.Profiles.FirstOrDefaultAsync(p => p.Auth0UserId == id);
    }}
    
    }