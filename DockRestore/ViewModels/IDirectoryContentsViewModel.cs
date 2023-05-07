using DockRestore.Infrastructure.Notification;
using DockRestore.Models;
using DockRestore.ViewModels.SelectedItemNotifications;

namespace DockRestore.ViewModels;

/// <summary>
/// Defines the contract for the view model responsible for handling the logic and data for the directory contents view.
/// </summary>
public interface IDirectoryContentsViewModel
{
    /// <summary>
    /// Gets the model instance containing the directory content information.
    /// </summary>
    IDirectoryContentInfo Model { get; }

    /// <summary>
    /// Gets the list of directories.
    /// </summary>
    List<DirectoryInfo> Directories { get; }

    /// <summary>
    /// Gets the list of files.
    /// </summary>
    List<FileInfo> Files { get; }

    /// <summary>
    /// Gets the list of all file system items.
    /// </summary>
    List<FileSystemInfo> FileSystemItems { get; }

    /// <summary>
    /// Gets or sets the currently selected file system item.
    /// </summary>
    FileSystemInfo SelectedItem { get; set; }

    /// <summary>
    /// Subscribes an <see cref="INotificationHandler{TNotification}"/> for ArchiveSelectedNotification to the list of subscribers.
    /// </summary>
    /// <param name="subscriber">The subscriber implementing the INotificationHandler for ArchiveSelectedNotification.</param>
    void Subscribe(INotificationHandler<IArchiveSelectedNotification> subscriber);

    /// <summary>
    /// Selects the next item in the list.
    /// </summary>
    void SelectNext();

    /// <summary>
    /// Selects the previous item in the list.
    /// </summary>
    void SelectPrevious();

    /// <summary>
    /// Selects the first item in the list.
    /// </summary>
    void SelectFirst();

    /// <summary>
    /// Selects the last item in the list.
    /// </summary>
    void SelectLast();

    /// <summary>
    /// Processes the selected item based on its type.
    /// </summary>
    void ProcessSelectedItem();

    /// <summary>
    /// Changes the parent directory to the specified directory and updates the selected item.
    /// </summary>
    /// <param name="directoryInfo">The <see cref="DirectoryInfo"/> instance representing the new parent directory.</param>
    void ChangeParentDirectory(DirectoryInfo directoryInfo);

    /// <summary>
    /// Notifies the application that an archive file has been selected.
    /// </summary>
    /// <param name="archive">A FileInfo object representing the selected archive file.</param>
    void NotifySelectedArchive(FileInfo archive);
}