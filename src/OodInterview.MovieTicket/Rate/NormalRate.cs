namespace OodInterview.MovieTicket.Rate;

/// <summary>
/// Normal rate pricing strategy for standard seats.
/// </summary>
public class NormalRate : IPricingStrategy
{
    public NormalRate(decimal price)
    {
        Price = price;
    }

    public decimal Price { get; }
}
