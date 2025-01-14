using System;

namespace ProfileService.Services.Products;

public interface IProductService{

    Task<IEnumerable<ProductDto>> GetProductsAsync();
}