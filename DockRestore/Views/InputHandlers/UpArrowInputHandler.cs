namespace DockRestore.Views.InputHandlers;

/// <inheritdoc/>
public class UpArrowInputHandler : IInputHandler
{
    /// <inheritdoc/>
    public bool CanHandle(ConsoleKey key) => key == ConsoleKey.UpArrow;

    /// <inheritdoc/>
    public async Task HandleAsync(IDirectoryContentsView view, CancellationToken cancellationToken)
    {
        await view.SelectPreviousAsync(cancellationToken);
    }
}