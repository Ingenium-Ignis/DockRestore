namespace DockRestore.Infrastructure.Notification;

/// <summary>
/// Represents a contract for handling notifications of a specified type.
/// </summary>
/// <typeparam name="TNotification">The type of notification to handle.</typeparam>
public interface INotificationHandler<in TNotification> where TNotification : INotification
{
    /// <summary>
    /// Handles the specified notification.
    /// </summary>
    /// <param name="notification">The notification to handle.</param>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task Handle(TNotification notification, CancellationToken cancellationToken);
}