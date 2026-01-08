using OodInterview.FileSearch.FileSystem;

namespace OodInterview.FileSearch;

/// <summary>
/// Main class responsible for performing file system searches.
/// Similar to Unix's find command.
/// </summary>
public class FileSearcher
{
    /// <summary>
    /// Performs a recursive search through the file system starting from root.
    /// Returns a list of files that match the given criteria.
    /// </summary>
    /// <param name="root">The root directory to start the search from.</param>
    /// <param name="criteria">The search criteria to match against.</param>
    /// <returns>A list of files matching the criteria.</returns>
    public List<FileEntry> Search(FileEntry root, FileSearchCriteria criteria)
    {
        // List to store matching files
        var result = new List<FileEntry>();

        // Stack to handle recursive traversal without actual recursion
        var recursionStack = new Stack<FileEntry>();

        // Start with the root directory
        recursionStack.Push(root);

        // Continue until we've processed all files
        while (recursionStack.Count > 0)
        {
            // Get the next file to process
            var next = recursionStack.Pop();

            // Check if the file matches our criteria
            if (criteria.IsMatch(next))
            {
                result.Add(next);
            }

            // Add all directory entries to the stack for processing
            foreach (var entry in next.Entries)
            {
                recursionStack.Push(entry);
            }
        }

        return result;
    }
}
