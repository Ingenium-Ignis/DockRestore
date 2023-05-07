using System.Globalization;
using DockRestore.Infrastructure.Notification;
using DockRestore.InputManager;
using DockRestore.ViewModels;
using DockRestore.ViewModels.SelectedItemNotifications;
using DockRestore.Views.InputHandlers;
using Spectre.Console;
using SharpCompress.Archives;

namespace DockRestore.Views;

/// <summary>
/// Represents the view for displaying the directory contents.
/// </summary>
public class DirectoryContentsView : NotificationHandler<InputNotification>, IDirectoryContentsView
{
    private const int TableHeaderFooterLines = 5;

    private readonly List<IInputHandler> _handlers = new()
    {
        new UpArrowInputHandler(),
        new DownArrowInputHandler(),
        new HomeInputHandler(),
        new EndInputHandler(),
        new PageUpInputHandler(),
        new PageDownInputHandler(),
        new EnterInputHandler(),
    };

    private readonly IDirectoryContentsViewModel _viewModel;
    private int _scrollPosition;
    private int _numberOfEntries;

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryContentsView"/> class.
    /// </summary>
    /// <param name="viewModel">The view model instance.</param>
    public DirectoryContentsView(IDirectoryContentsViewModel viewModel)
    {
        _viewModel = viewModel;
        _scrollPosition = 0;
    }

    private IEnumerable<FileSystemInfo> RenderedItems => _viewModel.FileSystemItems.Skip(_scrollPosition).Take(_numberOfEntries);

    /// <summary>
    /// Renders the Directory Contents View into the passed layout.
    /// </summary>
    /// <param name="layout">The layout where the view should be rendered.</param>
    /// <param name="height">The current height of the layout.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task UpdateAsync(Layout layout, int height)
    {
        _numberOfEntries = height - TableHeaderFooterLines;

        if (_numberOfEntries < 1)
        {
            layout.Update(new Markup("[yellow]Your terminal window's height is too small to display the content properly."
                                   + " Please resize the terminal window to increase its height for an optimal viewing experience.[/]"));

            return Task.CompletedTask;
        }

        var table = CreateTable();

        layout.Update(table);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task SelectPreviousAsync(CancellationToken cancellationToken)
    {
        _viewModel.SelectPrevious();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task SelectNextAsync(CancellationToken cancellationToken)
    {
        _viewModel.SelectNext();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task SelectFirstAsync(CancellationToken cancellationToken)
    {
        _viewModel.SelectFirst();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task SelectLastAsync(CancellationToken cancellationToken)
    {
        _viewModel.SelectLast();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task PageUpAsync(CancellationToken cancellationToken)
    {
        PageUp();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task PageDownAsync(CancellationToken cancellationToken)
    {
        PageDown();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task ProcessSelectedAsync(CancellationToken cancellationToken)
    {
        _viewModel.ProcessSelectedItem();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public void Subscribe(INotificationHandler<ArchiveSelectedNotification> subscriber)
    {
        _viewModel.Subscribe(subscriber);
    }

    /// <inheritdoc/>
    protected override async Task TryHandleNotificationAsync(InputNotification notification, CancellationToken cancellationToken)
    {
        var handler = _handlers.Find(h => h.CanHandle(notification.ConsoleKeyInfo.Key));

        if (handler != null)
        {
            await handler.HandleAsync(this, cancellationToken);
        }

        UpdateScrollPosition();
    }

    /// <inheritdoc/>
    protected override Task HandleNotificationExceptionAsync(Exception ex)
    {
        return Task.CompletedTask;
    }

    private Table CreateTable()
    {
        var table = new Table
        {
            Title = new TableTitle(
                $"Directory Contents: {_viewModel.Model.ParentDirectory.FullName}",
                new Style(foreground: Color.SpringGreen1, decoration: Decoration.Bold)),
            Expand = true,
            Border = TableBorder.Horizontal,
            BorderStyle = new Style(foreground: Color.SpringGreen1),
        };

        table.AddColumn(string.Empty)
            .AddColumn("Name")
            .AddColumn("Created")
            .AddColumn("Size", column => column.Alignment = Justify.Right);

        foreach (var item in RenderedItems)
        {
            AddRow(item, table);
        }

        return table;
    }

    private void AddRow(FileSystemInfo item, Table table)
    {
        const string folderEmoji = ":file_folder:";
        const string fileEmoji = ":page_facing_up:";
        const string archiveEmoji = ":package:";

        Color? color = item == _viewModel.SelectedItem ? Color.Yellow : null;
        var style = new Style(foreground: color);

        var name = new Text(item.Name, style);
        var creationTime = new Text(item.CreationTime.ToString(CultureInfo.InvariantCulture), style);
        var emoji = new Markup(folderEmoji, style);
        var size = new Text(string.Empty, style);

        if (item == _viewModel.Model.ParentDirectory)
        {
            name = new Text("..", style);
            creationTime = new Text(string.Empty, style);
        }

        if (item is FileInfo fileInfo)
        {
            emoji = ArchiveFactory.IsArchive(fileInfo.FullName, out _) ? new Markup(archiveEmoji, style) : new Markup(fileEmoji, style);

            size = new Text($"{Math.Round(fileInfo.Length / 1000d, 2):#.00} KB", style);
        }

        table.AddRow(emoji, name, creationTime, size);
    }

    private void PageUp()
    {
        var firstRenderedIndex = _viewModel.FileSystemItems.IndexOf(RenderedItems.First());
        var newFirstRenderedIndex = firstRenderedIndex - RenderedItems.Count();
        var clampedIndex = Math.Max(newFirstRenderedIndex, 0);

        var isFirstPage = clampedIndex == firstRenderedIndex;

        // Set the new selected item and update the scroll position
        SetSelectedItemAndUpdateScroll(clampedIndex, isFirstPage);
    }

    private void PageDown()
    {
        var lastRenderedIndex = _viewModel.FileSystemItems.IndexOf(RenderedItems.Last());
        var newLastRenderedIndex = lastRenderedIndex + RenderedItems.Count();
        var clampedIndex = Math.Min(newLastRenderedIndex, _viewModel.FileSystemItems.Count - 1);

        var isLastPage = clampedIndex == lastRenderedIndex;

        // Set the new selected item and update the scroll position
        SetSelectedItemAndUpdateScroll(clampedIndex, isLastPage);
    }

    private void SetSelectedItemAndUpdateScroll(int newIndex, bool selectExtremeItem)
    {
        var renderedIndex = RenderedItems.ToList().IndexOf(_viewModel.SelectedItem);

        // Set the new selected item and update the scroll position
        _viewModel.SelectedItem = _viewModel.FileSystemItems[newIndex];
        UpdateScrollPosition();

        // Set the new selected item in the RenderedItems list, based on the selectExtremeItem flag
        if (selectExtremeItem)
        {
            _viewModel.SelectedItem = newIndex == 0 ? RenderedItems.First() : RenderedItems.Last();
        }
        else
        {
            _viewModel.SelectedItem = RenderedItems.ElementAt(renderedIndex);
        }
    }

    private void UpdateScrollPosition()
    {
        _scrollPosition = CalculateScrollPosition(_viewModel.SelectedItem);
    }

    private int CalculateScrollPosition(FileSystemInfo selectedItem)
    {
        var selectedIndex = _viewModel.FileSystemItems.IndexOf(selectedItem);
        var lastVisibleIndex = _scrollPosition + _numberOfEntries - 1;

        if (selectedIndex < _scrollPosition)
        {
            return selectedIndex;
        }

        if (selectedIndex > lastVisibleIndex)
        {
            return selectedIndex - _numberOfEntries + 1;
        }

        return _scrollPosition;
    }
}