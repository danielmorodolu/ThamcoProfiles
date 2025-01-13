using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace ProfileService.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

   
    public IActionResult Index()
    {
        return View();
    }

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
        // Logout logic
        public async Task Logout()
        {
            try
            {
                await HttpContext.SignOutAsync("Auth0");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
                {
                    // Redirect to the home page after logout.
                    RedirectUri = Url.Action("Index", "Home")
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Logout: {ex.Message}");
            }

            // Redirect to Home page or Login page after logout
        }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
