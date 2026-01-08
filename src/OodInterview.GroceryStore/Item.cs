namespace OodInterview.GroceryStore;

/// <summary>
/// Represents a product item in the grocery store.
/// </summary>
public class Item
{
    /// <summary>
    /// Creates a new item with the specified details.
    /// </summary>
    /// <param name="name">Name of the item.</param>
    /// <param name="barcode">Unique barcode identifier.</param>
    /// <param name="category">Category of the item (e.g., Fruit, Candy).</param>
    /// <param name="price">Price of the item.</param>
    public Item(string name, string barcode, string category, decimal price)
    {
        Name = name;
        Barcode = barcode;
        Category = category;
        Price = price;
    }

    /// <summary>
    /// Gets the name of the item.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the unique barcode identifier.
    /// </summary>
    public string Barcode { get; }

    /// <summary>
    /// Gets the category of the item.
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Gets the price of the item.
    /// </summary>
    public decimal Price { get; }
}
