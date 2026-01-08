using OodInterview.FileSearch.FileSystem;

namespace OodInterview.FileSearch.Predicates;

/// <summary>
/// Base interface for all file search predicates.
/// </summary>
public interface IPredicate
{
    /// <summary>
    /// Checks if the given file matches the search condition.
    /// </summary>
    /// <param name="file">The file to check.</param>
    /// <returns>True if the file matches the predicate.</returns>
    bool IsMatch(FileEntry file);
}
