using DockRestore.Validator;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace DockRestore.FileRestoreManager;

/// <summary>
/// Provides functionality for restoring files from an archive to a specified directory.
/// </summary>
public class FileRestoreManager : IFileRestoreManager
{
    private readonly DirectoryInfo? _destinationDirectory;
    private readonly Exception? _constructorException;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="FileRestoreManager"/> class with the specified validator and destination directory.
    /// </summary>
    /// <param name="validator">An implementation of the <see cref="IValidator"/> interface.</param>
    /// <param name="destinationDirectory">The destination directory for extracted files.</param>
    public FileRestoreManager(IValidator validator, string destinationDirectory)
    {
        try
        {
            _destinationDirectory = validator.ValidateDirectory(destinationDirectory);

            if (_destinationDirectory.Exists) return;
            
            try
            {
                Directory.CreateDirectory(_destinationDirectory.FullName);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Directory {_destinationDirectory.FullName} doesn't exist and couldn't be created.", ex);
            }
        }
        catch (Exception ex)
        {
            _constructorException = ex;
        }
    }

    /// <inheritdoc/>
    public void RestoreFromArchive(FileInfo sourceArchive)
    {
        if (_constructorException is not null)
        {
            throw new Exception("File Restore Manager wasn't initialized correctly. See inner exception.",
                _constructorException);
        }

        using Stream stream = File.OpenRead(sourceArchive.FullName);
        using var reader = ReaderFactory.Open(stream);
        
        while (reader.MoveToNextEntry())
        {
            if (reader.Entry.IsDirectory) continue;
            
            var destination = GetDestinationDirectory(reader.Entry.Key);
            
            Directory.CreateDirectory(destination);
            
            reader.WriteEntryToDirectory(destination, new ExtractionOptions()
            {
                ExtractFullPath = false,
                Overwrite = true
            });
        }
    }

    /// <summary>
    /// Gets the destination directory for the given entry path.
    /// </summary>
    /// <param name="entryPath">The entry path of a file within the archive.</param>
    /// <returns>The destination directory for the given entry path.</returns>
    private string GetDestinationDirectory(string entryPath)
    {
        var directory = Path.GetDirectoryName(entryPath) ?? string.Empty;
        if (directory.StartsWith("/") || directory.StartsWith("\\"))
        {
            directory = directory.Substring(1);
        }

        var destination = Path.Combine(_destinationDirectory!.FullName, directory);
        return destination;
    }
}