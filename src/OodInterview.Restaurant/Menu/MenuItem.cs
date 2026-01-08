namespace OodInterview.Restaurant.Menu;

/// <summary>
/// Represents a single item available on the restaurant menu.
/// </summary>
public class MenuItem
{
    public MenuItem(string name, string description, decimal price, Category category)
    {
        Name = name;
        Description = description;
        Price = price;
        Category = category;
    }

    public string Name { get; }
    public string Description { get; }
    public decimal Price { get; }
    public Category Category { get; }
}
