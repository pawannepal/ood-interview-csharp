using OodInterview.FileSearch.FileSystem;
using OodInterview.FileSearch.Predicates;

namespace OodInterview.FileSearch;

/// <summary>
/// Wrapper class that encapsulates a search condition for file matching.
/// </summary>
public class FileSearchCriteria
{
    private readonly IPredicate _predicate;

    /// <summary>
    /// Creates a new search criteria with the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate defining the search criteria.</param>
    public FileSearchCriteria(IPredicate predicate)
    {
        _predicate = predicate;
    }

    /// <summary>
    /// Checks if the given file matches the search criteria.
    /// </summary>
    /// <param name="file">The file to check.</param>
    /// <returns>True if the file matches the criteria.</returns>
    public bool IsMatch(FileEntry file)
    {
        return _predicate.IsMatch(file);
    }
}
