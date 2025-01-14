using ProfileService.Models;
using ProfileService.Data;
namespace ProfileService.Services.Profiling;

public interface IProfileService
{
    Task<Profile?> GetUserByIdAsync(int? id);
    Task AddUserAsync(Profile user);
    Task<bool> UpdateUser(Profile user);
    bool UserExists(int id);
    Task SaveChangesAsync();
    Task<Profile?> GetUserByAuth0IdAsync(string? id);
}
