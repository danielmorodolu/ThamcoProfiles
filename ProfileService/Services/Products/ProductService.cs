using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ProfileService.Services.Products
{
    /// <summary>
    /// Service for interacting with the product API to fetch product details.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// Configures the HTTP client with base settings for communicating with the product API.
        /// </summary>
        /// <param name="client">An instance of <see cref="HttpClient"/> to perform HTTP operations.</param>
        /// <param name="configuration">The application configuration for retrieving API-related settings.</param>
        public ProductService(HttpClient client, IConfiguration configuration)
        {
            _configuration = configuration;

            var baseUrl = _configuration["WebServices:Products:BASEURL"] ?? string.Empty;
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(20);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = client;
        }

        /// <summary>
        /// Asynchronously retrieves an access token for authenticating API requests.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the access token as a string.</returns>
        public virtual async Task<string> GetAccessTokenAsync()
        {
            var tokenUrl = _configuration["WebServices:Auth0:TokenUrl"];
            var clientId = _configuration["WebServices:Auth0:ClientId"];
            var clientSecret = _configuration["WebServices:Auth0:ClientSecret"];
            var audience = _configuration["WebServices:Auth0:Audience"];

            var client = new HttpClient();

            var requestBody = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", clientId ?? string.Empty },
                { "client_secret", clientSecret ?? string.Empty },
                { "audience", audience ?? string.Empty }
            };

            var request = new FormUrlEncodedContent(requestBody);

            var response = await client.PostAsync(tokenUrl, request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            // Use System.Text.Json for deserialization
            var jsonResponse = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(responseBody);
            return jsonResponse.GetProperty("access_token").GetString() ?? string.Empty;
        }

        /// <summary>
        /// Asynchronously retrieves a collection of products from the API.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing a list of products.</returns>
        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await _client.GetAsync("/api/product/Products");
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                // Use Newtonsoft.Json for deserialization
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductDto>>(responseBody) ?? new List<ProductDto>();
            }
            catch (Exception ex)
            {
                // Log or handle exceptions appropriately
                Console.WriteLine($"Error fetching products: {ex.Message}");
                return new List<ProductDto>();
            }
        }
    }
}