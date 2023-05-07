namespace DockRestore.Views;

/// <summary>
/// Represents the interface for the restore process view.
/// </summary>
public interface IRestoreProcessView
{
    /// <summary>
    /// Displays the restore process view and handles the restoration logic.
    /// </summary>
    /// <param name="selectedArchive">The archive file to restore from.</param>
    Task ShowAsync(FileInfo selectedArchive);
}