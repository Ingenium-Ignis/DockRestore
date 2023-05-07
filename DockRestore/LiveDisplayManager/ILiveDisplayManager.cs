using DockRestore.Views;

namespace DockRestore.LiveDisplayManager;

/// <summary>
/// Provides an interface for managing live display updates.
/// </summary>
public interface ILiveDisplayManager
{
    /// <summary>
    /// Manages the live display updates of the specified refreshable view, updating it asynchronously.
    /// </summary>
    /// <param name="refreshableView">The refreshable view to be managed and updated.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ManageAsync(IRefreshableView refreshableView, CancellationToken cancellationToken);
}