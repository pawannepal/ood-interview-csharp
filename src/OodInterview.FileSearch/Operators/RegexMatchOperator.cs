using System.Text.RegularExpressions;

namespace OodInterview.FileSearch.Operators;

/// <summary>
/// Implements regular expression pattern matching for string values.
/// </summary>
public class RegexMatchOperator : IComparisonOperator<string>
{
    /// <inheritdoc />
    public bool IsMatch(string attributeValue, string expectedValue)
    {
        var pattern = new Regex(expectedValue);
        return pattern.IsMatch(attributeValue);
    }
}
