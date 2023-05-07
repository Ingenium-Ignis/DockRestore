namespace DockRestore.Models;

/// <inheritdoc/>
public class DirectoryContentInfo : IDirectoryContentInfo
{
    private string _directoryPath;
    private DirectoryInfo _parentDirectory;

    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryContentInfo"/> class.
    /// </summary>
    /// <param name="directoryPath">The path to the directory to load the content from.</param>
    public DirectoryContentInfo(string directoryPath)
    {
        _directoryPath = directoryPath;
        _parentDirectory = new DirectoryInfo(_directoryPath);
        LoadContents();
    }

    /// <inheritdoc/>
    public List<FileSystemInfo> Items { get; } = new();

    /// <inheritdoc/>
    public DirectoryInfo ParentDirectory
    {
        get => _parentDirectory;
        set
        {
            _parentDirectory = value;
            _directoryPath = _parentDirectory.FullName;
            LoadContents();
        }
    }
    
    /// <inheritdoc/>
    public IEnumerable<FileSystemInfo> GetOrderedFileSystemInfoList(
        Func<FileSystemInfo, object> orderBy, SortOrder sortOrder = SortOrder.Ascending)
    {
        return GetOrderedDirectoryInfoList(orderBy, sortOrder)
            .Concat((IEnumerable<FileSystemInfo>)GetOrderedFileInfoList(orderBy, sortOrder));
    }

    /// <inheritdoc/>
    public IEnumerable<DirectoryInfo> GetOrderedDirectoryInfoList(
        Func<DirectoryInfo, object> orderBy, SortOrder sortOrder = SortOrder.Ascending)
    {
        var directoryInfoList = Items.OfType<DirectoryInfo>();
        return sortOrder == SortOrder.Ascending
            ? directoryInfoList.OrderBy(orderBy)
            : directoryInfoList.OrderByDescending(orderBy);
    }

    /// <inheritdoc/>
    public IEnumerable<FileInfo> GetOrderedFileInfoList(
        Func<FileInfo, object> orderBy, SortOrder sortOrder = SortOrder.Ascending)
    {
        var fileInfoList = Items.OfType<FileInfo>();
        return sortOrder == SortOrder.Ascending
            ? fileInfoList.OrderBy(orderBy)
            : fileInfoList.OrderByDescending(orderBy);
    }

    /// <summary>
    /// Loads the contents of the directory specified by the <see cref="_directoryPath"/> property,
    /// adding the subdirectories and files to the <see cref="Items"/> collection.
    /// </summary>
    private void LoadContents()
    {
        Items.Clear();

        var directories = Directory.GetDirectories(_directoryPath);
        var files = Directory.GetFiles(_directoryPath);

        foreach (var dir in directories)
        {
            Items.Add(new DirectoryInfo(dir));
        }

        foreach (var file in files)
        {
            Items.Add(new FileInfo(file));
        }
    }
}