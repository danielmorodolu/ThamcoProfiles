using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ProfileService.Models;
using ProfileService.Services.Profiling;
using ProfileService.Services.Auth0;
using System.Diagnostics;

namespace ProfileService.Controllers
{
    /// <summary>
    /// Controller for managing user account-related operations.
    /// </summary>
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IProfileService _profileService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IConfiguration configuration, IProfileService profileService, ILogger<AccountController> logger)
        {
            _configuration = configuration;
            _profileService = profileService;
            _logger = logger;
        }

        /// <summary>
        /// Initiates the login process using Auth0.
        /// </summary>
        [HttpGet("Login")]
        public IActionResult Login()
        {
            try
            {
                return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "Auth0");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Login: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        /// <summary>
        /// Logs out the user and redirects to the home page.
        /// </summary>
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync("Auth0");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var redirectUri = Url.Action("Index", "Home") ?? "/";
                return Redirect(redirectUri);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Logout: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        /// <summary>
        /// Fetches and displays user profile details.
        /// </summary>
        [Authorize]
        [HttpGet("Details")]
        public async Task<IActionResult> Details()
        {
            try
            {
                var auth0UserId = Auth0UserHelper.GetAuth0UserId(User);
                var profile = await _profileService.GetUserByAuth0IdAsync(auth0UserId);
                var email = Auth0UserHelper.GetEmail(User);

                if (profile == null)
                {
                    profile = new Profile
                    {
                        Email = email ?? string.Empty,
                        Auth0UserId = auth0UserId,
                        Password = "Auth0PasswordSetHere"
                    };

                    await _profileService.AddUserAsync(profile);
                    await _profileService.SaveChangesAsync();
                }

                return View(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Details: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        /// <summary>
        /// Displays the edit field form.
        /// </summary>
        [HttpGet("EditField")]
        public async Task<IActionResult> EditField(string field)
        {
            try
            {
                var auth0UserId = Auth0UserHelper.GetAuth0UserId(User);
                var user = await _profileService.GetUserByAuth0IdAsync(auth0UserId);

                if (user == null) return NotFound();

                ViewBag.Field = field;
                ViewBag.FieldValue = field switch
                {
                    "FirstName" => user.FirstName ?? string.Empty,
                    "LastName" => user.LastName ?? string.Empty,
                    "PhoneNumber" => user.PhoneNumber ?? string.Empty,
                    "PaymentAddress" => user.PaymentAddress ?? string.Empty,
                    _ => throw new ArgumentException("Invalid field name.")
                };

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in EditField: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        /// <summary>
        /// Saves updates to a user profile field.
        /// </summary>
        [HttpPost("EditField")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditField(string field, string newValue)
        {
            try
            {
                var auth0UserId = Auth0UserHelper.GetAuth0UserId(User);
                var user = await _profileService.GetUserByAuth0IdAsync(auth0UserId);

                if (user == null) return NotFound();

                switch (field)
                {
                    case "FirstName": user.FirstName = newValue; break;
                    case "LastName": user.LastName = newValue; break;
                    case "PhoneNumber": user.PhoneNumber = newValue; break;
                    case "PaymentAddress": user.PaymentAddress = newValue; break;
                    default: throw new ArgumentException("Invalid field name.");
                }

                await _profileService.UpdateUser(user);
                await _profileService.SaveChangesAsync();

                return RedirectToAction(nameof(Details));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating field: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        /// <summary>
        /// Confirms account deletion.
        /// </summary>
        [HttpGet("Delete")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var auth0UserId = Auth0UserHelper.GetAuth0UserId(User);
                var user = await _profileService.GetUserByAuth0IdAsync(auth0UserId);

                return user == null ? NotFound() : View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Delete: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        /// <summary>
        /// Deletes the account and redirects to logout.
        /// </summary>
        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            try
            {
                var auth0UserId = Auth0UserHelper.GetAuth0UserId(User);
                var user = await _profileService.GetUserByAuth0IdAsync(auth0UserId);

                if (user != null)
                {
                    // Uncomment if deletion logic is implemented
                    // await _profileService.DeleteUser(user);
                    await _profileService.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Logout));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting account: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}