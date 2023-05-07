namespace DockRestore.Views.InputHandlers;

/// <inheritdoc/>
public class EnterInputHandler : IInputHandler
{
    /// <inheritdoc/>
    public bool CanHandle(ConsoleKey key) => key == ConsoleKey.Enter;

    /// <inheritdoc/>
    public async Task HandleAsync(IDirectoryContentsView view, CancellationToken cancellationToken)
    {
        await view.ProcessSelectedAsync(cancellationToken);
    }
}