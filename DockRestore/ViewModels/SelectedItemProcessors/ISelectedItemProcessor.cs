namespace DockRestore.ViewModels.SelectedItemProcessors;

/// <summary>
/// Defines the methods for processing selected file system items in the directory contents view model.
/// </summary>
public interface ISelectedItemProcessor
{
    /// <summary>
    /// Determines whether this processor can handle the specified file system item.
    /// </summary>
    /// <param name="item">The file system item to be processed.</param>
    /// <returns><c>true</c> if this processor can handle the specified item; otherwise, <c>false</c>.</returns>
    bool CanProcess(FileSystemInfo item);

    /// <summary>
    /// Processes the specified file system item using the provided view model instance.
    /// </summary>
    /// <param name="viewModel">The view model instance for handling the logic and data for the directory contents view.</param>
    /// <param name="item">The file system item to be processed.</param>
    void Process(IDirectoryContentsViewModel viewModel, FileSystemInfo item);
}