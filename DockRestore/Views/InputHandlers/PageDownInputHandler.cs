namespace DockRestore.Views.InputHandlers;

/// <inheritdoc/>
public class PageDownInputHandler : IInputHandler
{
    /// <inheritdoc/>
    public bool CanHandle(ConsoleKey key) => key == ConsoleKey.PageDown;

    /// <inheritdoc/>
    public async Task HandleAsync(IDirectoryContentsView view, CancellationToken cancellationToken)
    {
        await view.PageDownAsync(cancellationToken);
    }
}