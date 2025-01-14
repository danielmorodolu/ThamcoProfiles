using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ThamcoProfiles.Services.Support
{
    /// <summary>
    /// Extension methods for configuring SameSite cookie handling.
    /// </summary>
    public static class SameSiteServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the application to properly handle SameSite=None cookies 
        /// by ensuring compatibility with clients that do not fully support this mode.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the configuration to.</param>
        /// <returns>The updated IServiceCollection.</returns>
        public static IServiceCollection ConfigureSameSiteNoneCookies(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // Set minimum SameSite policy to Unspecified
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;

                // Apply custom handling when appending cookies
                options.OnAppendCookie = cookieContext => CheckSameSite(cookieContext.CookieOptions);

                // Apply custom handling when deleting cookies
                options.OnDeleteCookie = cookieContext => CheckSameSite(cookieContext.CookieOptions);
            });

            return services;
        }

        /// <summary>
        /// Checks and adjusts the SameSite property for cookies to ensure compatibility with 
        /// browsers that may not handle SameSite=None correctly when Secure is not enabled.
        /// </summary>
        /// <param name="options">The cookie options to inspect and modify.</param>
        private static void CheckSameSite(CookieOptions options)
        {
            // Adjust SameSite behavior if SameSite=None is set but Secure is false
            if (options.SameSite == SameSiteMode.None && !options.Secure)
            {
                options.SameSite = SameSiteMode.Unspecified;
            }
        }
    }
}