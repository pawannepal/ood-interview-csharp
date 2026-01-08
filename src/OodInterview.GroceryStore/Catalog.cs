namespace OodInterview.GroceryStore;

/// <summary>
/// Manages the store's product catalog.
/// </summary>
public class Catalog
{
    private readonly Dictionary<string, Item> _items = [];

    /// <summary>
    /// Updates or adds an item to the catalog.
    /// </summary>
    /// <param name="item">The item to add or update.</param>
    public void UpdateItem(Item item)
    {
        _items[item.Barcode] = item;
    }

    /// <summary>
    /// Removes an item from the catalog by its barcode.
    /// </summary>
    /// <param name="barcode">The barcode of the item to remove.</param>
    public void RemoveItem(string barcode)
    {
        _items.Remove(barcode);
    }

    /// <summary>
    /// Retrieves an item from the catalog by its barcode.
    /// </summary>
    /// <param name="barcode">The barcode of the item to retrieve.</param>
    /// <returns>The item, or null if not found.</returns>
    public Item? GetItem(string barcode)
    {
        return _items.GetValueOrDefault(barcode);
    }
}
