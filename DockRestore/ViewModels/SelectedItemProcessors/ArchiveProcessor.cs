using SharpCompress.Archives;

namespace DockRestore.ViewModels.SelectedItemProcessors;

/// <summary>
/// Processes archive file selections in the directory contents view model.
/// </summary>
public class ArchiveProcessor : ISelectedItemProcessor
{
    /// <inheritdoc/>
    public bool CanProcess(FileSystemInfo item)
    {
        if (!item.Exists) return false;
        if (item is not FileInfo file) return false;
        return ArchiveFactory.IsArchive(file.FullName, out _);
    }

    /// <inheritdoc/>
    public void Process(IDirectoryContentsViewModel viewModel, FileSystemInfo item)
    {
        if (!item.Exists) return;
        if (item is not FileInfo file) return;
        if (!ArchiveFactory.IsArchive(file.FullName, out _)) return;
        viewModel.NotifySelectedArchive(file);
    }
}