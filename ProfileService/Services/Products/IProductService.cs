using System;

namespace ThamcoProfiles.Services.Products;

public interface IProductService{

    Task<IEnumerable<ProductDto>> GetProductsAsync();
}