using OodInterview.FileSearch.FileSystem;

namespace OodInterview.FileSearch.Predicates;

/// <summary>
/// Implements logical AND operation between multiple predicates.
/// </summary>
public class AndPredicate : ICompositePredicate
{
    private readonly IReadOnlyList<IPredicate> _operands;

    /// <summary>
    /// Creates a new AND predicate with the specified predicates.
    /// </summary>
    /// <param name="operands">The predicates that must all match.</param>
    public AndPredicate(IReadOnlyList<IPredicate> operands)
    {
        _operands = operands;
    }

    /// <summary>
    /// Checks if the given file matches ALL predicates.
    /// </summary>
    /// <param name="file">The file to check.</param>
    /// <returns>True if all predicates match.</returns>
    public bool IsMatch(FileEntry file)
    {
        return _operands.All(predicate => predicate.IsMatch(file));
    }
}
