namespace DockRestore.Validator;

/// <summary>
/// Provides a set of validation methods for various input values.
/// </summary>
public class Validator : IValidator
{
    /// <summary>
    /// Validates the given directory path, ensuring that it is a valid directory path.
    /// </summary>
    /// <param name="path">The directory path to validate.</param>
    /// <returns>A <see cref="DirectoryInfo"/> instance representing the validated directory path.</returns>
    /// <exception cref="ArgumentException">Thrown when the path is invalid or empty.</exception>
    public DirectoryInfo ValidateDirectory(string path)
    {
        try
        {
            path = Path.GetFullPath(path);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("The destination directory must be a valid path", ex);
        }

        if (path == string.Empty)
        {
            throw new ArgumentException("The destination directory path can't be empty");
        }
        
        return new DirectoryInfo(path);
    }
}