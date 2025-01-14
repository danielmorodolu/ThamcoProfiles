using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;


namespace ProfileService.Services.Auth0
{
    public static class Auth0UserHelper
    {
       
        public static string? GetAuth0UserId(ClaimsPrincipal user)
        {
            // 'sub' is the claim type used for the Auth0 User ID
            return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        public static string? GetEmail(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }

        
    }
}