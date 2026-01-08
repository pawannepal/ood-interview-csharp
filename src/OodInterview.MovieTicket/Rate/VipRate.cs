namespace OodInterview.MovieTicket.Rate;

/// <summary>
/// VIP rate pricing strategy for VIP seats.
/// </summary>
public class VipRate : IPricingStrategy
{
    public VipRate(decimal price)
    {
        Price = price;
    }

    public decimal Price { get; }
}
