namespace OodInterview.ShippingLocker;

/// <summary>
/// Interface for notification services.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Sends a notification to a user.
    /// </summary>
    void SendNotification(string message, Account.Account user);
}
