using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProfileService.Services.Products
{
    /// <summary>
    /// A mock implementation of the <see cref="IProductService"/> interface, used for testing or development purposes.
    /// </summary>
    public class FakeProductService : IProductService
    {
        /// <summary>
        /// A predefined list of products for simulation purposes.
        /// </summary>
        private readonly List<ProductDto> _products;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeProductService"/> class with a predefined list of products.
        /// </summary>
        public FakeProductService()
        {
            // List of sample products for testing.
            _products = new List<ProductDto>
            {
                new ProductDto { Id = 101, Name = "Alpha Gadget", Description = "High-quality gadget for daily tasks.", Price = 19.99m },
                new ProductDto { Id = 202, Name = "Beta Toolset", Description = "A comprehensive toolkit for professionals.", Price = 49.99m },
                new ProductDto { Id = 303, Name = "Gamma Widget", Description = "An innovative widget to enhance productivity.", Price = 29.99m },
                new ProductDto { Id = 404, Name = "Delta Accessory", Description = "A stylish and practical accessory for tech enthusiasts.", Price = 12.49m },
                new ProductDto
                {
                    Id = 5,
                    Name = "Apple AirPods Pro (2nd Generation)",
                    Description = "Active noise cancellation, spatial audio, MagSafe charging.",
                    Price = 249.00m
                },
                new ProductDto
                {
                    Id = 3,
                    Name = "Sony WH-1000XM5",
                    Description = "Industry-leading noise cancellation, 30-hour battery life.",
                    Price = 399.99m
                }
            };
        }

        /// <summary>
        /// Asynchronously retrieves the list of predefined products.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> of <see cref="ProductDto"/>.
        /// </returns>
        public Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            // Return the predefined list of products as an asynchronous operation.
            return Task.FromResult<IEnumerable<ProductDto>>(_products);
        }
    }
}