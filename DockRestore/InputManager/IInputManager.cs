using DockRestore.Infrastructure.Notification;

namespace DockRestore.InputManager;

/// <summary>
/// Provides an interface for managing user input and dispatching input notifications.
/// </summary>
public interface IInputManager
{
    /// <summary>
    /// Subscribes an input notification handler to receive input notifications.
    /// </summary>
    /// <param name="subscriber">An <see cref="INotificationHandler{TNotification}"/> instance that handles <see cref="InputNotification"/> instances.</param>
    void Subscribe(INotificationHandler<InputNotification> subscriber);
    
    /// <summary>
    /// Runs the input manager, listening for user input and dispatching input notifications accordingly.
    /// </summary>
    /// <param name="cancellationTokenSource">A cancellation token source that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RunAsync(CancellationTokenSource cancellationTokenSource);
}