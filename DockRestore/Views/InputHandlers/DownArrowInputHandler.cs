namespace DockRestore.Views.InputHandlers;

/// <inheritdoc/>
public class DownArrowInputHandler : IInputHandler
{
    /// <inheritdoc/>
    public bool CanHandle(ConsoleKey key) => key == ConsoleKey.DownArrow;

    /// <inheritdoc/>
    public async Task HandleAsync(IDirectoryContentsView view, CancellationToken cancellationToken)
    {
        await view.SelectNextAsync(cancellationToken);
    }
}