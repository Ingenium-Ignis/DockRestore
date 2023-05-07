namespace DockRestore.Validator;

/// <summary>
/// Defines an interface for validating various input values.
/// </summary>
public interface IValidator
{
    /// <summary>
    /// Validates the given directory path, ensuring that it is a valid directory path.
    /// </summary>
    /// <param name="path">The directory path to validate.</param>
    /// <returns>A <see cref="DirectoryInfo"/> instance representing the validated directory path.</returns>
    /// <exception cref="ArgumentException">Thrown when the path is invalid or empty.</exception>
    DirectoryInfo ValidateDirectory(string path);
}