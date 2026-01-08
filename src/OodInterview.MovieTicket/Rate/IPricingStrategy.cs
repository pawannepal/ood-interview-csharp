namespace OodInterview.MovieTicket.Rate;

/// <summary>
/// Interface for pricing strategies that determine ticket prices.
/// </summary>
public interface IPricingStrategy
{
    /// <summary>
    /// Gets the price for this pricing strategy.
    /// </summary>
    decimal Price { get; }
}
