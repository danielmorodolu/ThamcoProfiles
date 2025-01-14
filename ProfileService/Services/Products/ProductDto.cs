namespace ProfileService.Services.Products;

/// <summary>
/// Represents a product with detailed information including identifiers, descriptions, price, and availability.
/// </summary>
public class ProductDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the product.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the European Article Number (EAN) for the product.
    /// </summary>
    public string? Ean { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the category of the product.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the name of the category the product belongs to.
    /// </summary>
    public string? CategoryName { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the brand of the product.
    /// </summary>
    public int BrandId { get; set; }

    /// <summary>
    /// Gets or sets the name of the brand associated with the product.
    /// </summary>
    public string? BrandName { get; set; }

    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the product is in stock.
    /// </summary>
    public bool InStock { get; set; }

    // Uncomment or modify this property if required in the future:
    // /// <summary>
    // /// Gets or sets the expected restock date for the product.
    // /// </summary>
    // public DateTime? ExpectedRestock { get; set; }
}