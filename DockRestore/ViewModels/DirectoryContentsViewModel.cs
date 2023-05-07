using System.Collections.Concurrent;
using DockRestore.Infrastructure.Notification;
using DockRestore.Models;
using DockRestore.ViewModels.SelectedItemNotifications;
using DockRestore.ViewModels.SelectedItemProcessors;

namespace DockRestore.ViewModels;

/// <summary>
/// Represents the view model for handling the logic and data for the directory contents view.
/// </summary>
public class DirectoryContentsViewModel : IDirectoryContentsViewModel
{
    private readonly ConcurrentBag<INotificationHandler<ArchiveSelectedNotification>> _subscribers;
    
    private readonly List<ISelectedItemProcessor> _processingStrategies = new()
    {
        new DirectoryProcessor(),
        new ArchiveProcessor()
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryContentsViewModel"/> class.
    /// </summary>
    /// <param name="model">The model instance.</param>
    public DirectoryContentsViewModel(IDirectoryContentInfo model)
    {
        _subscribers = new ConcurrentBag<INotificationHandler<ArchiveSelectedNotification>>();
        Model = model;
        SelectedItem = model.ParentDirectory;
    }

    /// <inheritdoc/>
    public IDirectoryContentInfo Model { get; }

    /// <inheritdoc/>
    public List<DirectoryInfo> Directories =>
        Model.GetOrderedDirectoryInfoList(x => x.CreationTimeUtc, SortOrder.Descending).ToList();

    /// <inheritdoc/>
    public List<FileInfo> Files =>
        Model.GetOrderedFileInfoList(x => x.CreationTimeUtc, SortOrder.Descending).ToList();

    /// <inheritdoc/>
    public List<FileSystemInfo> FileSystemItems => new List<FileSystemInfo>{ Model.ParentDirectory }
        .Concat(Model.GetOrderedFileSystemInfoList(x => x.CreationTimeUtc, SortOrder.Descending)).ToList();

    /// <inheritdoc/>
    public FileSystemInfo SelectedItem { get; set; }
    
    /// <inheritdoc/>
    public void Subscribe(INotificationHandler<ArchiveSelectedNotification> subscriber)
    {
        _subscribers.Add(subscriber);
    }

    /// <inheritdoc/>
    public void SelectNext()
    {
        var currentIndex = FileSystemItems.IndexOf(SelectedItem);

        // If the selected item is not in the list, select the first item.
        if (currentIndex < 0)
        {
            SelectedItem = FileSystemItems[0];
        }
        // If the selected item is not the last item, select the next item.
        else if (currentIndex < FileSystemItems.Count - 1)
        {
            SelectedItem = FileSystemItems[currentIndex + 1];
        }

        // Do nothing if the selected item is the last item.
    }

    /// <inheritdoc/>
    public void SelectPrevious()
    {
        var currentIndex = FileSystemItems.IndexOf(SelectedItem);

        // If the selected item is not in the list, select the first item.
        if (currentIndex < 0)
        {
            SelectedItem = FileSystemItems[0];
        }
        // If the selected item is not the first item, select the previous item.
        else if (currentIndex > 0)
        {
            SelectedItem = FileSystemItems[currentIndex - 1];
        }

        // Do nothing if the selected item is the first item.
    }

    /// <inheritdoc/>
    public void SelectFirst()
    {
        SelectedItem = FileSystemItems[0];
    }

    /// <inheritdoc/>
    public void SelectLast()
    {
        SelectedItem = FileSystemItems.Last();
    }
    
    /// <inheritdoc/>
    public void ProcessSelectedItem()
    {
        var strategy = _processingStrategies.FirstOrDefault(s => s.CanProcess(SelectedItem));
        strategy?.Process(this, SelectedItem);
    }
    
    /// <inheritdoc/>
    public void ChangeParentDirectory(DirectoryInfo directoryInfo)
    {
        if (directoryInfo == Model.ParentDirectory)
        {
            var original = Model.ParentDirectory;
            Model.ParentDirectory = directoryInfo.Parent ?? directoryInfo;
            SelectedItem = Model.Items.SingleOrDefault(x => x.FullName == original.FullName) ?? directoryInfo;
        }
        else
        {
            Model.ParentDirectory = directoryInfo;
            SelectedItem = Model.ParentDirectory;
        }
    }
    
    /// <inheritdoc/>
    public void NotifySelectedArchive(FileInfo archive)
    {
        var notification = new ArchiveSelectedNotification(archive);
        foreach (var subscriber in _subscribers)
        {
            subscriber.Handle(notification, new CancellationToken(false)).GetAwaiter().GetResult();
        }
    }
}