namespace DockRestore.Views.InputHandlers;

/// <summary>
/// Represents an input handler for the DirectoryContentsView class.
/// </summary>
public interface IInputHandler
{
    /// <summary>
    /// Determines whether this input handler can handle the specified console key.
    /// </summary>
    /// <param name="key">The console key to check.</param>
    /// <returns>true if this input handler can handle the specified key; otherwise, false.</returns>
    bool CanHandle(ConsoleKey key);
    
    /// <summary>
    /// Asynchronously handles the input for the specified view.
    /// </summary>
    /// <param name="view">The IDirectoryContentsView instance to handle input for.</param>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task HandleAsync(IDirectoryContentsView view, CancellationToken cancellationToken);
}