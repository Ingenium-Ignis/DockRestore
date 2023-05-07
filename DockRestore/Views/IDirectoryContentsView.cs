using DockRestore.Infrastructure.Notification;
using DockRestore.InputManager;
using DockRestore.ViewModels.SelectedItemNotifications;
using Spectre.Console;

namespace DockRestore.Views;

public interface IDirectoryContentsView : IRefreshableView, INotificationHandler<InputNotification>
{
    /// <summary>
    /// Asynchronously selects the previous item in the list.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SelectPreviousAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Asynchronously selects the next item in the list.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SelectNextAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Asynchronously selects the first item in the list.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SelectFirstAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Asynchronously selects the last item in the list.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SelectLastAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Asynchronously moves the view one page up.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task PageUpAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Asynchronously moves the view one page down.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task PageDownAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Asynchronously processes the currently selected item.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ProcessSelectedAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Subscribes an <see cref="INotificationHandler{TNotification}"/> for ArchiveSelectedNotification to the list of subscribers.
    /// </summary>
    /// <param name="subscriber">The subscriber implementing the INotificationHandler for ArchiveSelectedNotification.</param>
    void Subscribe(INotificationHandler<ArchiveSelectedNotification> subscriber);
}