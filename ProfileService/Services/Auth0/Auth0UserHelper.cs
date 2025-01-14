using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;

namespace ProfileService.Services.Auth0
{
    /// <summary>
    /// A helper class for extracting Auth0-related claims from a user's principal.
    /// </summary>
    public static class Auth0UserHelper
    {
        /// <summary>
        /// Retrieves the Auth0 User ID from the claims of a given user.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the authenticated user.</param>
        /// <returns>
        /// A string representing the Auth0 User ID if found; otherwise, <c>null</c>.
        /// </returns>
        public static string? GetAuth0UserId(ClaimsPrincipal user)
        {
            // 'sub' (Subject) is typically the claim type for the Auth0 User ID.
            return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// Retrieves the email address from the claims of a given user.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> representing the authenticated user.</param>
        /// <returns>
        /// A string representing the email address if found; otherwise, <c>null</c>.
        /// </returns>
        public static string? GetEmail(ClaimsPrincipal user)
        {
            // Extract the email from claims using the Email claim type.
            return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }
    }
}