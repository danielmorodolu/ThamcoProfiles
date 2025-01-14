using ProfileService.Models;
using ProfileService.Data;

namespace ProfileService.Services.Profiling;

/// <summary>
/// Interface defining the contract for profile management operations.
/// </summary>
public interface IProfileService
{
    /// <summary>
    /// Retrieves a user profile by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user profile.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the user profile if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Profile?> GetUserByIdAsync(int? id);

    /// <summary>
    /// Adds a new user profile to the database.
    /// </summary>
    /// <param name="user">The user profile to be added.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddUserAsync(Profile user);

    /// <summary>
    /// Updates an existing user profile in the database.
    /// </summary>
    /// <param name="user">The user profile containing updated information.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation, returning <c>true</c> if the update was successful; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> UpdateUser(Profile user);

    /// <summary>
    /// Checks if a user profile exists in the database based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user profile.</param>
    /// <returns><c>true</c> if the user exists; otherwise, <c>false</c>.</returns>
    bool UserExists(int id);

    /// <summary>
    /// Persists all pending changes to the database.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SaveChangesAsync();

    /// <summary>
    /// Retrieves a user profile by its Auth0 unique identifier.
    /// </summary>
    /// <param name="id">The Auth0 unique identifier of the user profile.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the user profile if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Profile?> GetUserByAuth0IdAsync(string? id);
}