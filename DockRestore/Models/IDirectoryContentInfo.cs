namespace DockRestore.Models;

/// <summary>
/// Represents the model for loading and storing directory content information.
/// </summary>
public interface IDirectoryContentInfo
{
    /// <summary>
    /// Gets the list of file system info items.
    /// </summary>
    List<FileSystemInfo> Items { get; }
    
    /// <summary>
    /// Gets or sets the parent directory information.
    /// </summary>
    DirectoryInfo ParentDirectory { get; set; }

    /// <summary>
    /// Gets an ordered list of FileSystemInfo objects based on the provided orderBy function and sortOrder (first directories, then files).
    /// </summary>
    /// <param name="orderBy">A function to extract a key from an element.</param>
    /// <param name="sortOrder">The sorting order (ascending or descending).</param>
    /// <returns>An IEnumerable of FileSystemInfo objects ordered by the specified key and order.</returns>
    IEnumerable<FileSystemInfo> GetOrderedFileSystemInfoList(
        Func<FileSystemInfo, object> orderBy, SortOrder sortOrder = SortOrder.Ascending);

    /// <summary>
    /// Gets an ordered list of DirectoryInfo objects based on the provided orderBy function and sortOrder.
    /// </summary>
    /// <param name="orderBy">A function to extract a key from an element.</param>
    /// <param name="sortOrder">The sorting order (ascending or descending).</param>
    /// <returns>An IEnumerable of DirectoryInfo objects ordered by the specified key and order.</returns>
    IEnumerable<DirectoryInfo> GetOrderedDirectoryInfoList(
        Func<DirectoryInfo, object> orderBy, SortOrder sortOrder = SortOrder.Ascending);

    /// <summary>
    /// Gets an ordered list of FileInfo objects based on the provided orderBy function and sortOrder.
    /// </summary>
    /// <param name="orderBy">A function to extract a key from an element.</param>
    /// <param name="sortOrder">The sorting order (ascending or descending).</param>
    /// <returns>An IEnumerable of FileInfo objects ordered by the specified key and order.</returns>
    IEnumerable<FileInfo> GetOrderedFileInfoList(
        Func<FileInfo, object> orderBy, SortOrder sortOrder = SortOrder.Ascending);
}