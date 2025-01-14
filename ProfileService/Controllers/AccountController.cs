using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using ProfileService.Models;
using ProfileService.Services.Profiling;
using ProfileService.Services.Auth0;
using System.Diagnostics;


namespace ProfileService.Controllers
{

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

        [Route("Account/Login")]

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
        

        public async Task Logout()
    {
    try
    {
       // Sign out from Auth0 and the cookie scheme
        await HttpContext.SignOutAsync("Auth0");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        // Redirect to the home page after logout
        var redirectUri = Url.Action("Index", "Home");
        Response.Redirect(redirectUri);

       
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error in Logout: {ex.Message}");
    }
}

[Authorize]
[HttpGet]
public async Task<IActionResult> Details()
{
    try
    {
        var auth0UserId = Auth0UserHelper.GetAuth0UserId(User);
        var user = await _profileService.GetUserByAuth0IdAsync(auth0UserId);
        var userEmail = Auth0UserHelper.GetEmail(User);

        if(user==null){

                    user = new Profile
                    {
                        Id = 1,
                        Email = userEmail ?? "",
                        Auth0UserId = auth0UserId,
                        Password = "Auth0PasswordSetHere", // You can handle password reset with Auth0
                    };

                    await _profileService.AddUserAsync(user);
                    await _profileService.SaveChangesAsync();
            }

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }
    catch (Exception ex)
    {
        _logger.LogError($"Error in Details: {ex.Message}");
        return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}


        // Edit profile fields
        [HttpGet]
        public async Task<IActionResult> EditField(string field)
        {
            try
            {
                var auth0UserId = Auth0UserHelper.GetAuth0UserId(User);
                var user = await _profileService.GetUserByAuth0IdAsync(auth0UserId);

                if (user == null)
                {
                    return NotFound();
                }

                ViewBag.Field = field;
                
                ViewBag.FieldValue = field switch
                {
                    "FirstName" => user.FirstName ?? string.Empty,
                    "LastName" => user.LastName ?? string.Empty,
                    "PhoneNumber" => user.PhoneNumber ?? string.Empty,
                    "PaymentAddress" => user.PaymentAddress ?? string.Empty,
                    _ => throw new Exception("Invalid field.")
                };

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in EditField: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditField(string field, string newValue)
        {
            try
            {
                var auth0UserId = Auth0UserHelper.GetAuth0UserId(User);
                var user = await _profileService.GetUserByAuth0IdAsync(auth0UserId);
                

                if (user == null)
                {
                    return NotFound();
                }

                switch (field)
                {
                    case "FirstName":
                        user.FirstName = newValue;
                        break;
                    case "LastName":
                        user.LastName = newValue;
                        break;
                    case "PhoneNumber":
                        user.PhoneNumber = newValue;
                        break;
                    case "PaymentAddress":
                        user.PaymentAddress = newValue;
                        break;
                    default:
                        throw new Exception("Invalid field.");
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

        // Delete account
        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var auth0UserId = Auth0UserHelper.GetAuth0UserId(User);
                var user = await _profileService.GetUserByAuth0IdAsync(auth0UserId);

                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Delete: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed()
        {
            try
            {
                var auth0UserId = Auth0UserHelper.GetAuth0UserId(User);
                var user = await _profileService.GetUserByAuth0IdAsync(auth0UserId);

                if (user != null)
                {
                    //await _profileService.DeleteUser(user);
                    await _profileService.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Logout));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user: {ex.Message}");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        // Check if a user exists
        private bool UserExists(int id)
        {
            return _profileService.UserExists(id);
        }

        // Test route to check if the controller works
        [HttpGet]
        public IActionResult Test()
        {
            return Ok("AccountController is working");
        }
    }
}
