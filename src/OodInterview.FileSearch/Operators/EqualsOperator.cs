namespace OodInterview.FileSearch.Operators;

/// <summary>
/// Implements exact equality comparison between values.
/// </summary>
/// <typeparam name="T">The type of values being compared.</typeparam>
public class EqualsOperator<T> : IComparisonOperator<T>
{
    /// <inheritdoc />
    public bool IsMatch(T attributeValue, T expectedValue)
    {
        return Equals(attributeValue, expectedValue);
    }
}
