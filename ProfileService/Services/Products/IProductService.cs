using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProfileService.Services.Products;

/// <summary>
/// Defines the contract for product-related operations.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Asynchronously retrieves a collection of products.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> of <see cref="ProductDto"/>.
    /// </returns>
    Task<IEnumerable<ProductDto>> GetProductsAsync();
}