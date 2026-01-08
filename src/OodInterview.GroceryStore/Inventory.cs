namespace OodInterview.GroceryStore;

/// <summary>
/// Tracks product stock levels.
/// </summary>
public class Inventory
{
    private readonly Dictionary<string, int> _stock = [];

    /// <summary>
    /// Adds or updates stock quantity for an item.
    /// </summary>
    /// <param name="barcode">The barcode of the item.</param>
    /// <param name="count">The quantity to add.</param>
    public void AddStock(string barcode, int count)
    {
        _stock[barcode] = _stock.GetValueOrDefault(barcode, 0) + count;
    }

    /// <summary>
    /// Reduces stock quantity for an item.
    /// </summary>
    /// <param name="barcode">The barcode of the item.</param>
    /// <param name="count">The quantity to reduce.</param>
    public void ReduceStock(string barcode, int count)
    {
        _stock[barcode] = _stock.GetValueOrDefault(barcode, 0) - count;
    }

    /// <summary>
    /// Gets the current stock quantity for an item.
    /// </summary>
    /// <param name="barcode">The barcode of the item.</param>
    /// <returns>The current stock quantity.</returns>
    public int GetStock(string barcode)
    {
        return _stock.GetValueOrDefault(barcode, 0);
    }
}
