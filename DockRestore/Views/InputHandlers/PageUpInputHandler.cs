namespace DockRestore.Views.InputHandlers;

/// <inheritdoc/>
public class PageUpInputHandler : IInputHandler
{
    /// <inheritdoc/>
    public bool CanHandle(ConsoleKey key) => key == ConsoleKey.PageUp;

    /// <inheritdoc/>
    public async Task HandleAsync(IDirectoryContentsView view, CancellationToken cancellationToken)
    {
        await view.PageUpAsync(cancellationToken);
    }
}