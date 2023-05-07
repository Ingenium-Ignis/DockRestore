namespace DockRestore.ViewModels.SelectedItemProcessors;

/// <summary>
/// Processes directory selections in the directory contents view model.
/// </summary>
public class DirectoryProcessor : ISelectedItemProcessor
{
    /// <inheritdoc/>
    public bool CanProcess(FileSystemInfo item)
    {
        return item is DirectoryInfo;
    }

    /// <inheritdoc/>
    public void Process(IDirectoryContentsViewModel viewModel, FileSystemInfo item)
    {
        if (item is not DirectoryInfo directoryInfo) return;
        
        viewModel.ChangeParentDirectory(directoryInfo);
    }
}