using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ProfileService.Services.Products
{
    public class FakeProductService : IProductService
    {
        private readonly List<ProductDto> _products;

        public FakeProductService()
        {
            _products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product A", Description = "Description A", Price = 10.99m },
                new ProductDto { Id = 2, Name = "Product B", Description = "Description B", Price = 15.49m },
                new ProductDto { Id = 3, Name = "Product C", Description = "Description C", Price = 7.99m }
            };
        }

        public Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            return Task.FromResult<IEnumerable<ProductDto>>(_products);
        }
    }
}