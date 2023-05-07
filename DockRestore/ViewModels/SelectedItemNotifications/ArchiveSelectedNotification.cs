using DockRestore.Infrastructure.Notification;
using SharpCompress.Archives;

namespace DockRestore.ViewModels.SelectedItemNotifications;

/// <summary>
/// Represents a notification that occurs when an archive is selected.
/// </summary>
public class ArchiveSelectedNotification : INotification
{
    /// <summary>
    /// Initializes a new instance of the ArchiveSelectedNotification class with the specified archive.
    /// </summary>
    /// <param name="archive">The selected archive file.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided archive file does not exist in the file system or is not an archive.
    /// </exception>
    public ArchiveSelectedNotification(FileInfo archive)
    {
        if (!archive.Exists)
        {
            throw new ArgumentException("Selected archive file must exist in file system.");
        }

        if (!ArchiveFactory.IsArchive(archive.FullName, out _))
        {
            throw new ArgumentException("Selected file must be an archive.");
        }
        
        Archive = archive;
    }
    
    /// <summary>
    /// Gets the selected archive file.
    /// </summary>
    public FileInfo Archive { get; }
}