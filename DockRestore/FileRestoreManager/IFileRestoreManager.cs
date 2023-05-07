namespace DockRestore.FileRestoreManager;

/// <summary>
/// Provides functionality for restoring files from an archive to a specified directory.
/// </summary>
public interface IFileRestoreManager
{
    /// <summary>
    /// Restores files from the specified archive to the destination directory.
    /// </summary>
    /// <param name="sourceArchive">A <see cref="FileInfo"/> object representing the archive file to be extracted.</param>
    void RestoreFromArchive(FileInfo sourceArchive);
}