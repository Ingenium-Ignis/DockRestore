namespace DockRestore.Infrastructure.Notification;

/// <summary>
/// Provides an abstract base class for safely handling notifications.
/// </summary>
/// <typeparam name="TNotification">The type of the notification being handled.</typeparam>
public abstract class NotificationHandler<TNotification> : INotificationHandler<TNotification> 
    where TNotification : INotification
{
    /// <summary>
    /// Handles the specified notification safely by invoking <see cref="TryHandleNotificationAsync"/> and
    /// <see cref="HandleNotificationExceptionAsync"/> in case of exceptions.
    /// </summary>
    /// <param name="notification">The notification to handle.</param>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        try
        {
            await TryHandleNotificationAsync(notification, cancellationToken);
        }
        catch (Exception ex)
        {
            await HandleNotificationExceptionAsync(ex);
        }
    }

    /// <summary>
    /// Tries to handle the notification asynchronously. This method should contain the actual logic for handling the notification.
    /// </summary>
    /// <param name="notification">The notification to handle.</param>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected abstract Task TryHandleNotificationAsync(TNotification notification, CancellationToken cancellationToken);
    
    /// <summary>
    /// Handles exceptions that occur while handling the notification.
    /// </summary>
    /// <param name="ex">The exception that occurred while handling the notification.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected abstract Task HandleNotificationExceptionAsync(Exception ex);
}