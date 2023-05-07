namespace DockRestore.Views.InputHandlers;

/// <inheritdoc/>
public class HomeInputHandler : IInputHandler
{
    /// <inheritdoc/>
    public bool CanHandle(ConsoleKey key) => key == ConsoleKey.Home;

    /// <inheritdoc/>
    public async Task HandleAsync(IDirectoryContentsView view, CancellationToken cancellationToken)
    {
        await view.SelectFirstAsync(cancellationToken);
    }
}