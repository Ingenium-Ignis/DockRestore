using DockRestore.Infrastructure.Notification;

namespace DockRestore.ViewModels.SelectedItemNotifications;

public interface IArchiveSelectedNotification : INotification
{
    /// <summary>
    /// Gets the selected archive file.
    /// </summary>
    FileInfo Archive { get; }
}