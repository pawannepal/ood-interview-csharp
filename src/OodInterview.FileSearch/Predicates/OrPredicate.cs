using OodInterview.FileSearch.FileSystem;

namespace OodInterview.FileSearch.Predicates;

/// <summary>
/// Implements logical OR operation between multiple predicates.
/// </summary>
public class OrPredicate : ICompositePredicate
{
    private readonly IReadOnlyList<IPredicate> _operands;

    /// <summary>
    /// Creates a new OR predicate with the specified predicates.
    /// </summary>
    /// <param name="operands">The predicates where at least one must match.</param>
    public OrPredicate(IReadOnlyList<IPredicate> operands)
    {
        _operands = operands;
    }

    /// <summary>
    /// Checks if the given file matches ANY predicate.
    /// </summary>
    /// <param name="file">The file to check.</param>
    /// <returns>True if at least one predicate matches.</returns>
    public bool IsMatch(FileEntry file)
    {
        return _operands.Any(predicate => predicate.IsMatch(file));
    }
}
