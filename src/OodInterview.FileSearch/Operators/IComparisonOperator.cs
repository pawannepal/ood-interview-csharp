namespace OodInterview.FileSearch.Operators;

/// <summary>
/// Base interface for all comparison operations in the file search system.
/// </summary>
/// <typeparam name="T">The type of values being compared.</typeparam>
public interface IComparisonOperator<in T>
{
    /// <summary>
    /// Checks if the attribute value matches the expected value.
    /// </summary>
    /// <param name="attributeValue">The actual value from the file.</param>
    /// <param name="expectedValue">The expected value to match against.</param>
    /// <returns>True if the values match according to this operator.</returns>
    bool IsMatch(T attributeValue, T expectedValue);
}
