using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using ProfileService.Services.Products;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace ProfileService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICompositeViewEngine _viewEngine;

        public HomeController(ILogger<HomeController> logger, IProductService productService, ICompositeViewEngine viewEngine)
        {
            _logger = logger;
            _productService = productService;
            _viewEngine = viewEngine;
        }

        // Homepage: Displays a list of products
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _productService.GetProductsAsync(); // Ensure the service returns a valid list
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching products: {ex.Message}");
                return View(new List<ProductDto>()); // Pass an empty list on error
            }
        }


        // Privacy policy page
        public IActionResult Privacy()
        {
            return View();
        }

        // Error handling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
[Route("Home/Search")]
public async Task<IActionResult> Search(string query)
{
    IEnumerable<ProductDto> products = null!;

    try
    {
        // Get products from the service
        products = await _productService.GetProductsAsync();

        // Safely filter products based on the query
        if (!string.IsNullOrEmpty(query))
        {
            products = products
                ?.Where(p => p?.Name?.Contains(query, StringComparison.OrdinalIgnoreCase) == true) // Null-safe
                ?? Enumerable.Empty<ProductDto>();
        }
    }
    catch (Exception ex)
    {
        _logger.LogWarning($"Failed to access product service: {ex.Message}");
        products = Enumerable.Empty<ProductDto>(); // Fallback to an empty list
    }

    if (!ViewExists("Search"))
    {
        return View("Error");
    }
    
    return View(products);
}


        // Utility method to check if a view exists
        private bool ViewExists(string viewName)
        {
            var result = _viewEngine.FindView(ControllerContext, viewName, false);
            return result.View != null;
        }
    }
}
