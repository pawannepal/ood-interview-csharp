namespace OodInterview.Restaurant.Menu;

/// <summary>
/// Manages the restaurant's menu items.
/// </summary>
public class Menu
{
    private readonly Dictionary<string, MenuItem> _menuItems = [];

    /// <summary>
    /// Adds a new item to the menu.
    /// </summary>
    /// <param name="item">The menu item to add.</param>
    public void AddItem(MenuItem item)
    {
        _menuItems[item.Name] = item;
    }

    /// <summary>
    /// Gets a menu item by name.
    /// </summary>
    /// <param name="name">The name of the menu item.</param>
    /// <returns>The menu item, or null if not found.</returns>
    public MenuItem? GetItem(string name)
    {
        return _menuItems.GetValueOrDefault(name);
    }

    /// <summary>
    /// Gets all menu items.
    /// </summary>
    public IReadOnlyDictionary<string, MenuItem> MenuItems => _menuItems.AsReadOnly();
}
