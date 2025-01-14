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
    /// <summary>
    /// HomeController manages the main application logic, including the homepage, privacy policy, and error handling.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICompositeViewEngine _viewEngine;

        /// <summary>
        /// Constructor initializes logger, product service, and view engine dependencies.
        /// </summary>
        public HomeController(
            ILogger<HomeController> logger,
            IProductService productService,
            ICompositeViewEngine viewEngine)
        {
            _logger = logger;
            _productService = productService;
            _viewEngine = viewEngine;
        }

        /// <summary>
        /// Displays the homepage with a list of products.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                // Retrieve products using the service
                var products = await _productService.GetProductsAsync() ?? new List<ProductDto>();
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving product list: {ex.Message}");
                return View(Enumerable.Empty<ProductDto>()); // Return an empty list on failure
            }
        }

        /// <summary>
        /// Renders the privacy policy page.
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Handles application errors by displaying an error page.
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        /// <summary>
        /// Searches products based on a user query.
        /// </summary>
        [HttpGet]
        [Route("Home/Search")]
        public async Task<IActionResult> Search(string query)
        {
            IEnumerable<ProductDto> filteredProducts;

            try
            {
                // Fetch all products
                var products = await _productService.GetProductsAsync();

                // Filter products safely using LINQ
                filteredProducts = string.IsNullOrWhiteSpace(query)
                    ? products
                    : products.Where(p => p?.Name?.Contains(query, StringComparison.OrdinalIgnoreCase) == true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Search operation failed: {ex.Message}");
                filteredProducts = Enumerable.Empty<ProductDto>();
            }

            // Check if the "Search" view exists before rendering
            return ViewExists("Search") ? View(filteredProducts) : View("Error");
        }

        /// <summary>
        /// Utility method to verify if a view exists within the current controller context.
        /// </summary>
        /// <param name="viewName">Name of the view to check.</param>
        /// <returns>True if the view exists; otherwise, false.</returns>
        private bool ViewExists(string viewName)
        {
            var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
            return viewResult.View != null;
        }
    }
}