namespace OodInterview.ShippingLocker;

/// <summary>
/// Implementation of INotificationService that sends email notifications.
/// </summary>
public class EmailNotificationService : INotificationService
{
    /// <summary>
    /// Sends an email notification to the user.
    /// </summary>
    public void SendNotification(string message, Account.Account user)
    {
        // In a real implementation, this would send an email
        // For now, we just simulate the email being sent
        Console.WriteLine($"Email sent to {user.OwnerName}: {message}");
    }
}
