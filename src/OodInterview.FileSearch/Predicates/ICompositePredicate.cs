namespace OodInterview.FileSearch.Predicates;

/// <summary>
/// Marker interface to identify predicates that combine multiple other predicates (AND, OR, NOT).
/// </summary>
public interface ICompositePredicate : IPredicate
{
}
