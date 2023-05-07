using DockRestore.Infrastructure.Notification;
using DockRestore.ViewModels.SelectedItemNotifications;

namespace DockRestore.ConsoleCommands;

/// <summary>
/// Handles the ArchiveSelectedNotification event by extending the SafeNotificationHandler class.
/// </summary>
public class ArchiveSelectedNotificationHandler : NotificationHandler<ArchiveSelectedNotification>, IArchiveSelectedNotificationHandler
{
    /// <inheritdoc/>
    public event EventHandler<ArchiveSelectedNotification>? ArchiveSelected;
    
    /// <inheritdoc/>
    protected override Task TryHandleNotificationAsync(ArchiveSelectedNotification notification, CancellationToken cancellationToken)
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
    private void OnArchiveSelected(ArchiveSelectedNotification e)
    {
        ArchiveSelected?.Invoke(this, e);
    }
}