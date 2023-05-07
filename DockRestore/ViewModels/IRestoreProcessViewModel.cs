namespace DockRestore.ViewModels;

/// <summary>
/// Represents the view model for handling the logic and data for the restore process view.
/// </summary>
public interface IRestoreProcessViewModel
{
    /// <summary>
    /// Runs the pre-action processor and returns its success status.
    /// </summary>
    /// <returns><c>true</c> if the pre-action processor succeeded; otherwise, <c>false</c>.</returns>
    Task<bool> RunPreActionProcessorAsync();
    
    /// <summary>
    /// Restores the contents from the specified archive file.
    /// </summary>
    /// <param name="selectedArchive">The archive file to restore contents from.</param>
    void RestoreFromArchive(FileInfo selectedArchive);
    
    /// <summary>
    /// Runs the post-action processor and returns its success status.
    /// </summary>
    /// <returns><c>true</c> if the post-action processor succeeded; otherwise, <c>false</c>.</returns>
    Task<bool> RunPostActionProcessorAsync();
    
    /// <summary>
    /// Determines if the Docker container manager is a null implementation.
    /// </summary>
    /// <returns><c>true</c> if the Docker container manager is a null implementation; otherwise, <c>false</c>.</returns>
    bool IsDockerContainerManagerNull();
}