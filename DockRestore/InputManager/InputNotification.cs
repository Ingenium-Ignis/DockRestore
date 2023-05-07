using DockRestore.Infrastructure.Notification;

namespace DockRestore.InputManager;

/// <summary>
/// Represents a notification containing user input information.
/// </summary>
public class InputNotification : INotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InputNotification"/> class with the specified console key information.
    /// </summary>
    /// <param name="consoleKeyInfo">The console key information associated with the user input.</param>
    public InputNotification(ConsoleKeyInfo consoleKeyInfo)
    {
        ConsoleKeyInfo = consoleKeyInfo;
    }

    /// <summary>
    /// Gets the console key information associated with the user input.
    /// </summary>
    public ConsoleKeyInfo ConsoleKeyInfo { get; }
}