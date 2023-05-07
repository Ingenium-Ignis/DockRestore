using Docker.DotNet.Models;

namespace DockRestore.Docker;

/// <summary>
/// Represents a processor for performing post-action processing on Docker containers.
/// </summary>
public interface IPostActionContainerProcessor
{
    /// <summary>
    /// Processes the specified Docker containers after applying the filters.
    /// </summary>
    /// <param name="containers">The Docker containers to process.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains a boolean value indicating whether the processing was successful.</returns>
    Task<bool> ProcessAsync(IEnumerable<ContainerListResponse> containers);
}