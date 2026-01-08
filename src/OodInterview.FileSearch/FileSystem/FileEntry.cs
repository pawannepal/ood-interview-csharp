namespace OodInterview.FileSearch.FileSystem;

/// <summary>
/// Represents a file or directory in the file system.
/// Contains basic file attributes and supports hierarchical structure.
/// </summary>
public class FileEntry
{
    private readonly HashSet<FileEntry> _entries = [];

    /// <summary>
    /// Creates a new file with the specified attributes.
    /// </summary>
    /// <param name="isDirectory">Whether this entry is a directory.</param>
    /// <param name="size">The size of the file in bytes.</param>
    /// <param name="owner">The owner of the file.</param>
    /// <param name="filename">The name of the file.</param>
    public FileEntry(bool isDirectory, int size, string owner, string filename)
    {
        IsDirectory = isDirectory;
        Size = size;
        Owner = owner;
        Filename = filename;
    }

    /// <summary>
    /// Gets whether this entry is a directory.
    /// </summary>
    public bool IsDirectory { get; }

    /// <summary>
    /// Gets the size of the file in bytes.
    /// </summary>
    public int Size { get; }

    /// <summary>
    /// Gets the owner of the file.
    /// </summary>
    public string Owner { get; }

    /// <summary>
    /// Gets the name of the file.
    /// </summary>
    public string Filename { get; }

    /// <summary>
    /// Gets the directory entries (files and subdirectories).
    /// </summary>
    public IReadOnlySet<FileEntry> Entries => _entries;

    /// <summary>
    /// Extracts the value of a specified file attribute.
    /// </summary>
    /// <param name="attribute">The attribute to extract.</param>
    /// <returns>The value of the attribute.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid attribute is specified.</exception>
    public object Extract(FileAttribute attribute)
    {
        return attribute switch
        {
            FileAttribute.IsDirectory => IsDirectory,
            FileAttribute.Size => Size,
            FileAttribute.Owner => Owner,
            FileAttribute.Filename => Filename,
            _ => throw new ArgumentException("Invalid filter criteria type", nameof(attribute))
        };
    }

    /// <summary>
    /// Adds a file or directory entry to this directory.
    /// </summary>
    /// <param name="entry">The entry to add.</param>
    public void AddEntry(FileEntry entry)
    {
        _entries.Add(entry);
    }
}
