namespace DockRestore.Views.InputHandlers;

/// <inheritdoc/>
public class EndInputHandler : IInputHandler
{
    /// <inheritdoc/>
    public bool CanHandle(ConsoleKey key) => key == ConsoleKey.End;
    
    /// <inheritdoc/>
    public async Task HandleAsync(IDirectoryContentsView view, CancellationToken cancellationToken)
    {
        await view.SelectLastAsync(cancellationToken);
    }
}