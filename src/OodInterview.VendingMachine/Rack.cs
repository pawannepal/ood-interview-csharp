namespace OodInterview.VendingMachine;

/// <summary>
/// Represents a rack (slot) in the vending machine that holds a specific product.
/// </summary>
public class Rack
{
    /// <summary>
    /// Gets the unique rack code (e.g., "A1", "A2").
    /// </summary>
    public string RackCode { get; }

    /// <summary>
    /// Gets the product stored in this rack.
    /// </summary>
    public Product Product { get; }

    /// <summary>
    /// Gets or sets the current product count in the rack.
    /// </summary>
    public int ProductCount { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Rack class.
    /// </summary>
    /// <param name="rackCode">The unique rack code.</param>
    /// <param name="product">The product to store.</param>
    /// <param name="count">The initial product count.</param>
    public Rack(string rackCode, Product product, int count)
    {
        RackCode = rackCode;
        Product = product;
        ProductCount = count;
    }

    /// <summary>
    /// Sets the product count.
    /// </summary>
    /// <param name="count">The new count.</param>
    public void SetCount(int count)
    {
        ProductCount = count;
    }

    public override string ToString()
    {
        return $"Rack{{Code={RackCode}, Product={Product.ProductCode}, Count={ProductCount}}}";
    }
}
