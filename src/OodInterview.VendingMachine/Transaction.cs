namespace OodInterview.VendingMachine;

/// <summary>
/// Represents a completed transaction in the vending machine.
/// </summary>
public class Transaction
{
    /// <summary>
    /// Gets or sets the rack from which the product was dispensed.
    /// </summary>
    public Rack? Rack { get; set; }

    /// <summary>
    /// Gets or sets the product that was purchased.
    /// </summary>
    public Product? Product { get; set; }

    /// <summary>
    /// Gets or sets the total amount (change returned to customer).
    /// </summary>
    public decimal TotalAmount { get; set; }

    public override string ToString()
    {
        return $"Transaction{{Product={Product?.ProductCode}, Rack={Rack?.RackCode}, TotalAmount={TotalAmount:C}}}";
    }
}
