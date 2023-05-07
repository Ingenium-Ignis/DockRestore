using DockRestore.Views;
using Spectre.Console;

namespace DockRestore.LiveDisplayManager;

/// <summary>
/// Manages live display updates for a refreshable view.
/// </summary>
public class LiveDisplayManager : ILiveDisplayManager
{
    /// <summary>
    /// Manages the live display updates of the specified refreshable view, updating it asynchronously.
    /// </summary>
    /// <param name="refreshableView">The refreshable view to be managed and updated.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task ManageAsync(IRefreshableView refreshableView, CancellationToken cancellationToken)
    {
        AnsiConsole.AlternateScreen(() => RefreshLoopAsync(refreshableView, cancellationToken).GetAwaiter().GetResult());
        
        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Executes the refresh loop for the specified refreshable view, updating its content asynchronously.
    /// </summary>
    /// <param name="refreshableView">The refreshable view to be updated.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private static async Task RefreshLoopAsync(IRefreshableView refreshableView, CancellationToken cancellationToken)
    {
        const double framerate = 60d;
        const int msDelay = (int)((1 / framerate) * 1000);

        while (!cancellationToken.IsCancellationRequested)
        {
            var consoleHeight = AnsiConsole.Console.Profile.Height;
            var consoleWidth = AnsiConsole.Console.Profile.Width;

            AnsiConsole.Clear();

            var layout = new Layout();

            var live = new LiveDisplay(AnsiConsole.Console, layout);
            await live.StartAsync(async ctx =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        await refreshableView.UpdateAsync(layout, consoleHeight);

                        ctx.Refresh();

                        if (consoleHeight != AnsiConsole.Console.Profile.Height || 
                            consoleWidth != AnsiConsole.Console.Profile.Width)
                        {
                            break;
                        }

                        try
                        {
                            await Task.Delay(msDelay, cancellationToken);
                        }
                        catch (TaskCanceledException)
                        {
                            // Ignore.
                        }
                    }
                }
            );
        }
    }
}