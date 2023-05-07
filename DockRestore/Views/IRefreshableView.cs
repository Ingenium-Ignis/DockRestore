using Spectre.Console;

namespace DockRestore.Views;

/// <summary>
/// Defines the contract for a refreshable view that can be updated asynchronously.
/// </summary>
public interface IRefreshableView
{
    /// <summary>
    /// Asynchronously updates the view within the specified layout and console height.
    /// </summary>
    /// <param name="layout">The layout in which the view is displayed.</param>
    /// <param name="consoleHeight">The height of the console.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(Layout layout, int consoleHeight);
}