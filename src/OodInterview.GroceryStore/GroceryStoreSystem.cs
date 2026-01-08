using OodInterview.GroceryStore.Discount;

namespace OodInterview.GroceryStore;

/// <summary>
/// Main system class that coordinates all grocery store operations.
/// </summary>
public class GroceryStoreSystem
{
    private readonly Catalog _catalog;
    private readonly Inventory _inventory;
    private readonly List<DiscountCampaign> _activeDiscounts = [];
    private readonly Checkout _checkout;

    /// <summary>
    /// Creates a new grocery store system with default components.
    /// </summary>
    public GroceryStoreSystem()
    {
        _catalog = new Catalog();
        _inventory = new Inventory();
        _checkout = new Checkout(_activeDiscounts);
    }

    /// <summary>
    /// Creates a new grocery store system with custom components.
    /// </summary>
    /// <param name="catalog">The product catalog.</param>
    /// <param name="inventory">The inventory tracker.</param>
    /// <param name="activeDiscounts">The list of active discount campaigns.</param>
    public GroceryStoreSystem(Catalog catalog, Inventory inventory, List<DiscountCampaign> activeDiscounts)
    {
        _catalog = catalog;
        _inventory = inventory;
        _activeDiscounts = activeDiscounts;
        _checkout = new Checkout(_activeDiscounts);
    }

    /// <summary>
    /// Adds or updates an item in the catalog.
    /// </summary>
    /// <param name="item">The item to add or update.</param>
    public void AddOrUpdateItem(Item item)
    {
        _catalog.UpdateItem(item);
    }

    /// <summary>
    /// Updates the inventory count for an item.
    /// </summary>
    /// <param name="barcode">The barcode of the item.</param>
    /// <param name="count">The quantity to add.</param>
    public void UpdateInventory(string barcode, int count)
    {
        _inventory.AddStock(barcode, count);
    }

    /// <summary>
    /// Adds a new discount campaign to the system.
    /// </summary>
    /// <param name="discount">The discount campaign to add.</param>
    public void AddDiscountCampaign(DiscountCampaign discount)
    {
        _activeDiscounts.Add(discount);
    }

    /// <summary>
    /// Returns the checkout system.
    /// </summary>
    public Checkout Checkout => _checkout;

    /// <summary>
    /// Retrieves an item from the catalog by its barcode.
    /// </summary>
    /// <param name="barcode">The barcode of the item.</param>
    /// <returns>The item, or null if not found.</returns>
    public Item? GetItemByBarcode(string barcode)
    {
        return _catalog.GetItem(barcode);
    }

    /// <summary>
    /// Removes an item from the catalog.
    /// </summary>
    /// <param name="barcode">The barcode of the item to remove.</param>
    public void RemoveItem(string barcode)
    {
        _catalog.RemoveItem(barcode);
    }
}
