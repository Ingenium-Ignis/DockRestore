using DockRestore.Infrastructure.Notification;
using DockRestore.ViewModels.SelectedItemNotifications;

namespace DockRestore.ConsoleCommands;

/// <summary>
/// Represents a handler for the <see cref="ArchiveSelectedNotification"/>.
/// </summary>
public interface IArchiveSelectedNotificationHandler : INotificationHandler<ArchiveSelectedNotification>
{
    /// <summary>
    /// Occurs when an archive is selected.
    /// </summary>
    event EventHandler<ArchiveSelectedNotification>? ArchiveSelected;
}