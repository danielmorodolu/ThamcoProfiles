using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json;
using ProfileService.Services.Products;
using ProfileService.Models;

namespace ProfileService.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync("api/product/Products");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var products = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ProductDto>>(jsonString);
            return products ?? Enumerable.Empty<ProductDto>();
        }
    }
}