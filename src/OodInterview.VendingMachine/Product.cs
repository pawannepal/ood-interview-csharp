namespace OodInterview.VendingMachine;

/// <summary>
/// Represents a product in the vending machine.
/// </summary>
public class Product
{
    /// <summary>
    /// Gets the unique product code.
    /// </summary>
    public string ProductCode { get; }

    /// <summary>
    /// Gets the product description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; }

    /// <summary>
    /// Initializes a new instance of the Product class.
    /// </summary>
    /// <param name="productCode">The unique product code.</param>
    /// <param name="description">The product description.</param>
    /// <param name="unitPrice">The unit price.</param>
    public Product(string productCode, string description, decimal unitPrice)
    {
        ProductCode = productCode;
        Description = description;
        UnitPrice = unitPrice;
    }

    public override string ToString()
    {
        return $"Product{{Code={ProductCode}, Description={Description}, Price={UnitPrice:C}}}";
    }
}
