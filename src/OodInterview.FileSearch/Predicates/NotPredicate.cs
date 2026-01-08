using OodInterview.FileSearch.FileSystem;

namespace OodInterview.FileSearch.Predicates;

/// <summary>
/// Implements logical NOT operation on a predicate.
/// </summary>
public class NotPredicate : ICompositePredicate
{
    private readonly IPredicate _operand;

    /// <summary>
    /// Creates a new NOT predicate with the specified predicate to negate.
    /// </summary>
    /// <param name="operand">The predicate to negate.</param>
    public NotPredicate(IPredicate operand)
    {
        _operand = operand;
    }

    /// <summary>
    /// Checks if the given file does NOT match the predicate.
    /// </summary>
    /// <param name="file">The file to check.</param>
    /// <returns>True if the predicate does not match.</returns>
    public bool IsMatch(FileEntry file)
    {
        return !_operand.IsMatch(file);
    }
}
