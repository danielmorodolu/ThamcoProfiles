using Microsoft.EntityFrameworkCore;
using ProfileService.Models;
using ProfileService.Data;

namespace ProfileService.Services.Profiling
{
    /// <summary>
    /// Service implementation for managing user profiles using Entity Framework.
    /// </summary>
    public class RealProfileService : IProfileService
    {
        private readonly ProfileContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RealProfileService"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing profile data.</param>
        public RealProfileService(ProfileContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<Profile?> GetUserByIdAsync(int? id)
        {
            // Fetches a profile by its unique identifier
            return await _context.Profile.FindAsync(id);
        }

        /// <inheritdoc/>
        public async Task AddUserAsync(Profile user)
        {
            // Adds a new profile to the database
            await _context.Profile.AddAsync(user);
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateUser(Profile user)
        {
            // Updates an existing profile with new information
            var existingUser = await _context.Profile.FindAsync(user.Id);
            if (existingUser == null) return false;

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.PaymentAddress = user.PaymentAddress;

            return true;
        }

        /// <inheritdoc/>
        public bool UserExists(int id)
        {
            // Checks if a profile exists with the given ID
            return _context.Profile.Any(p => p.Id == id);
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync()
        {
            // Saves all pending changes to the database
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<Profile?> GetUserByAuth0IdAsync(string? id)
        {
            // Fetches a profile by its Auth0 unique identifier
            return await _context.Profile.FirstOrDefaultAsync(p => p.Auth0UserId == id);
        }
    }
}