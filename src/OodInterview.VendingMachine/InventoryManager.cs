namespace OodInterview.VendingMachine;

/// <summary>
/// Manages the inventory of products in the vending machine.
/// </summary>
public class InventoryManager
{
    private Dictionary<string, Rack> _racks;

    /// <summary>
    /// Initializes a new instance of the InventoryManager class.
    /// </summary>
    public InventoryManager()
    {
        _racks = new Dictionary<string, Rack>();
    }

    /// <summary>
    /// Gets the product stored in the specified rack.
    /// </summary>
    /// <param name="rackCode">The rack code.</param>
    /// <returns>The product in the rack.</returns>
    public Product GetProductInRack(string rackCode)
    {
        return _racks[rackCode].Product;
    }

    /// <summary>
    /// Dispenses a product from the specified rack by decrementing the count.
    /// </summary>
    /// <param name="rack">The rack to dispense from.</param>
    public void DispenseProductFromRack(Rack rack)
    {
        rack.SetCount(rack.ProductCount - 1);
    }

    /// <summary>
    /// Updates the racks in the inventory.
    /// </summary>
    /// <param name="racks">The new rack configuration.</param>
    public void UpdateRack(Dictionary<string, Rack> racks)
    {
        _racks = racks;
    }

    /// <summary>
    /// Gets the rack with the specified code.
    /// </summary>
    /// <param name="name">The rack code.</param>
    /// <returns>The rack.</returns>
    public Rack GetRack(string name)
    {
        return _racks[name];
    }

    public override string ToString()
    {
        return $"InventoryManager{{Racks=[{string.Join(", ", _racks.Values)}]}}";
    }
}
