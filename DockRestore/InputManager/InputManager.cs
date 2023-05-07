using Spectre.Console;
using System.Collections.Concurrent;
using DockRestore.Infrastructure.Notification;

namespace DockRestore.InputManager;

/// <summary>
/// Manages user input and dispatches input notifications to subscribed handlers.
/// </summary>
public class InputManager : IInputManager
{
    private readonly ConcurrentBag<INotificationHandler<InputNotification>> _subscribers;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="InputManager"/> class.
    /// </summary>
    public InputManager()
    {
        _subscribers = new ConcurrentBag<INotificationHandler<InputNotification>>();
    }
    
    /// <inheritdoc/>
    public void Subscribe(INotificationHandler<InputNotification> subscriber)
    {
        _subscribers.Add(subscriber);
    }

    /// <inheritdoc/>
    public async Task RunAsync(CancellationTokenSource cancellationTokenSource)
    {
        ConsoleKeyInfo? consoleKeyInfo = null;

        do
        {
            await Task.Run(() => consoleKeyInfo = AnsiConsole.Console.Input.ReadKey(true));

            if (!consoleKeyInfo.HasValue) continue;
            
            var notification = new InputNotification(consoleKeyInfo.Value);
            foreach (var subscriber in _subscribers)
            {
                await subscriber.Handle(notification, cancellationTokenSource.Token);
            }
        }
        while (consoleKeyInfo?.Key != ConsoleKey.Escape);

        cancellationTokenSource.Cancel();
    }
}