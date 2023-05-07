using DockRestore.ConsoleCommands;
using DockRestore.ConsoleCommands.Settings;
using DockRestore.InputManager;
using DockRestore.LiveDisplayManager;
using DockRestore.Views;
using Moq;
using Spectre.Console.Cli;

namespace DockRestore.Tests.ConsoleCommands;

public class DefaultCommandTests
{
    [Test]
    public async Task ExecuteAsync_NoArchiveSelected_ReturnsZero()
    {
        // Arrange
        var mockDirectoryContentsView = new Mock<IDirectoryContentsView>();
        var mockInputManager = new Mock<IInputManager>();
        var mockArchiveSelectedNotificationHandler = new Mock<IArchiveSelectedNotificationHandler>();
        var mockLiveDisplayManager = new Mock<ILiveDisplayManager>();
        var mockRestoreProcessView = new Mock<IRestoreProcessView>();

        // Configure mockLiveDisplayManager to return immediately without invoking the ArchiveSelected event
        mockLiveDisplayManager
            .Setup(l => l.ManageAsync(mockDirectoryContentsView.Object, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var defaultCommand = new DefaultCommand(
            mockDirectoryContentsView.Object,
            mockInputManager.Object,
            mockArchiveSelectedNotificationHandler.Object,
            mockLiveDisplayManager.Object,
            mockRestoreProcessView.Object);

        // Create an instance of CommandContext
        var remainingArguments = new Mock<IRemainingArguments>();
        var commandContext = new CommandContext(remainingArguments.Object, "DefaultCommand", null);
        var defaultCommandSettings = new DefaultCommandSettings();

        // Act
        var result = await defaultCommand.ExecuteAsync(commandContext, defaultCommandSettings);

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
}