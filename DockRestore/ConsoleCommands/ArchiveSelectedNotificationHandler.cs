using DockRestore.Infrastructure.Notification;
using DockRestore.ViewModels.SelectedItemNotifications;

namespace DockRestore.ConsoleCommands;

/// <summary>
/// Handles the ArchiveSelectedNotification event by extending the SafeNotificationHandler class.
/// </summary>
public class ArchiveSelectedNotificationHandler : NotificationHandler<IArchiveSelectedNotification>, IArchiveSelectedNotificationHandler
{
    /// <inheritdoc/>
    public event EventHandler<IArchiveSelectedNotification>? ArchiveSelected;
    
    /// <inheritdoc/>
    protected override Task TryHandleNotificationAsync(IArchiveSelectedNotification notification, CancellationToken cancellationToken)
    {
        OnArchiveSelected(notification);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    protected override Task HandleNotificationExceptionAsync(Exception ex)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Raises the ArchiveSelected event.
    /// </summary>
    /// <param name="e">The ArchiveSelectedNotification containing event data.</param>
    private void OnArchiveSelected(IArchiveSelectedNotification e)
    {
        ArchiveSelected?.Invoke(this, e);
    }
}