namespace OodInterview.FileSearch.Operators;

/// <summary>
/// Implements greater than comparison for numeric values.
/// </summary>
/// <typeparam name="T">The numeric type being compared.</typeparam>
public class GreaterThanOperator<T> : IComparisonOperator<T> where T : IComparable<T>
{
    /// <inheritdoc />
    public bool IsMatch(T attributeValue, T expectedValue)
    {
        return attributeValue.CompareTo(expectedValue) > 0;
    }
}
