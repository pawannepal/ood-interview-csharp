using OodInterview.FileSearch.FileSystem;
using OodInterview.FileSearch.Operators;

namespace OodInterview.FileSearch.Predicates;

/// <summary>
/// Represents a basic predicate that compares a file attribute with an expected value.
/// </summary>
/// <typeparam name="T">The type of the value being compared.</typeparam>
public class SimplePredicate<T> : IPredicate
{
    private readonly FileAttribute _attributeName;
    private readonly IComparisonOperator<T> _operator;
    private readonly T _expectedValue;

    /// <summary>
    /// Creates a new simple predicate with the specified attribute, operator, and expected value.
    /// </summary>
    /// <param name="attributeName">The name of the file attribute to check.</param>
    /// <param name="comparisonOperator">The operator to use for comparison.</param>
    /// <param name="expectedValue">The expected value to compare against.</param>
    public SimplePredicate(
        FileAttribute attributeName,
        IComparisonOperator<T> comparisonOperator,
        T expectedValue)
    {
        _attributeName = attributeName;
        _operator = comparisonOperator;
        _expectedValue = expectedValue;
    }

    /// <inheritdoc />
    public bool IsMatch(FileEntry file)
    {
        // Extract the actual value of the attribute from the file
        var actualValue = file.Extract(_attributeName);

        // Check if the actual value is of the correct type
        if (actualValue is T typedValue)
        {
            // Perform the comparison using the specified operator
            return _operator.IsMatch(typedValue, _expectedValue);
        }

        return false;
    }
}
