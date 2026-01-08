namespace OodInterview.MovieTicket.Rate;

/// <summary>
/// Premium rate pricing strategy for premium seats.
/// </summary>
public class PremiumRate : IPricingStrategy
{
    public PremiumRate(decimal price)
    {
        Price = price;
    }

    public decimal Price { get; }
}
