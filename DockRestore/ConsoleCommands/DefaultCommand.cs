using DockRestore.ConsoleCommands.Settings;
using DockRestore.InputManager;
using DockRestore.LiveDisplayManager;
using DockRestore.Views;
using JetBrains.Annotations;
using Spectre.Console.Cli;

namespace DockRestore.ConsoleCommands;

/// <summary>
/// Represents the default command executed by the application.
/// </summary>
[UsedImplicitly]
public class DefaultCommand : AsyncCommand<DefaultCommandSettings>
{
    private readonly IDirectoryContentsView _directoryContentsView;
    private readonly IInputManager _inputManager;
    private readonly IArchiveSelectedNotificationHandler _archiveSelectedNotificationHandler;
    private readonly ILiveDisplayManager _liveDisplayManager;
    private readonly IRestoreProcessView _restoreProcessView;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultCommand"/> class.
    /// </summary>
    /// <param name="directoryContentsView">An instance of <see cref="IDirectoryContentsView"/> to display the directory contents.</param>
    /// <param name="inputManager">An instance of <see cref="IInputManager"/> to manage user input.</param>
    /// <param name="archiveSelectedNotificationHandler">An instance of <see cref="IArchiveSelectedNotificationHandler"/> to handle archive selection events.</param>
    /// <param name="liveDisplayManager">An instance of <see cref="ILiveDisplayManager"/> to manage the live display of directory contents.</param>
    /// <param name="restoreProcessView">An instance of <see cref="IRestoreProcessView"/> to display the restore process view.</param>
    public DefaultCommand(
        IDirectoryContentsView directoryContentsView, 
        IInputManager inputManager, 
        IArchiveSelectedNotificationHandler archiveSelectedNotificationHandler, 
        ILiveDisplayManager liveDisplayManager, 
        IRestoreProcessView restoreProcessView)
    {
        _directoryContentsView = directoryContentsView;
        _inputManager = inputManager;
        _archiveSelectedNotificationHandler = archiveSelectedNotificationHandler;
        _liveDisplayManager = liveDisplayManager;
        _restoreProcessView = restoreProcessView;
    }

    /// <summary>
    /// Executes the default command asynchronously.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="settings">The command settings.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the exit code for the command.</returns>
    public override async Task<int> ExecuteAsync(CommandContext context, DefaultCommandSettings settings)
    {
        var cancellationTokenSource = new CancellationTokenSource();

        FileInfo? selectedArchive = null;

        // When an archive is selected, store its information and cancel the CancellationTokenSource.
        _archiveSelectedNotificationHandler.ArchiveSelected += (_, notification) =>
        {
            selectedArchive = notification.Archive;
            cancellationTokenSource.Cancel();
        };
        _directoryContentsView.Subscribe(_archiveSelectedNotificationHandler);

        // Start listening for user input.
        _inputManager.Subscribe(_directoryContentsView);
        _ = _inputManager.RunAsync(cancellationTokenSource);

        // Start the live display manager to update the directory contents view and let the user select an archive.
        await _liveDisplayManager.ManageAsync(_directoryContentsView, cancellationTokenSource.Token);

        // If no archive is selected, exit with code 0.
        if (selectedArchive is null) return await Task.FromResult(0);

        // Start the restore process view to restore the selected archive.
        await _restoreProcessView.ShowAsync(selectedArchive);

        return await Task.FromResult(0);
    }
}