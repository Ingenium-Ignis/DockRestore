using DockRestore.Docker;
using DockRestore.FileRestoreManager;

namespace DockRestore.ViewModels;

/// <summary>
/// Represents the view model for handling the logic and data for the restore process view.
/// </summary>
public class RestoreProcessViewModel : IRestoreProcessViewModel
{
    private readonly IDockerContainerManager _dockerContainerManager;
    private readonly IPreActionContainerProcessor _preActionProcessor;
    private readonly IFileRestoreManager _fileRestoreManager;
    private readonly IPostActionContainerProcessor _postActionProcessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="RestoreProcessViewModel"/> class.
    /// </summary>
    /// <param name="dockerContainerManager">The Docker container manager instance.</param>
    /// <param name="preActionProcessor">The pre-action processor instance.</param>
    /// <param name="fileRestoreManager">The file restore manager instance.</param>
    /// <param name="postActionProcessor">The post-action processor instance.</param>
    public RestoreProcessViewModel(
        IDockerContainerManager dockerContainerManager,
        IPreActionContainerProcessor preActionProcessor,
        IFileRestoreManager fileRestoreManager,
        IPostActionContainerProcessor postActionProcessor)
    {
        _dockerContainerManager = dockerContainerManager;
        _preActionProcessor = preActionProcessor;
        _fileRestoreManager = fileRestoreManager;
        _postActionProcessor = postActionProcessor;
    }

    /// <inheritdoc/>
    public async Task<bool> RunPreActionProcessorAsync()
    {
        return await _preActionProcessor.ProcessAsync(await _dockerContainerManager.ListContainersAsync());
    }

    /// <inheritdoc/>
    public void RestoreFromArchive(FileInfo selectedArchive)
    {
        _fileRestoreManager.RestoreFromArchive(selectedArchive);
    }

    /// <inheritdoc/>
    public async Task<bool> RunPostActionProcessorAsync()
    {
        return await _postActionProcessor.ProcessAsync(await _dockerContainerManager.ListContainersAsync());
    }

    /// <inheritdoc/>
    public bool IsDockerContainerManagerNull()
    {
        return _dockerContainerManager is INullDockerContainerManager;
    }
}