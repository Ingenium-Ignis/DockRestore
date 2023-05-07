using Docker.DotNet.Models;

namespace DockRestore.Docker;

/// <summary>
/// Provides methods for managing Docker containers.
/// </summary>
public interface IDockerContainerManager
{
    /// <summary>
    /// Asynchronously lists all Docker containers.
    /// </summary>
    /// <returns>A list of Docker containers.</returns>
    Task<IList<ContainerListResponse>> ListContainersAsync();
    
    /// <summary>
    /// Gets the current container ID.
    /// </summary>
    /// <returns>The ID of the current container.</returns>
    string GetCurrentContainerId();
    
    /// <summary>
    /// Asynchronously sends a command to a Docker container and returns the exit code.
    /// </summary>
    /// <param name="container">The target Docker container.</param>
    /// <param name="command">The command to send.</param>
    /// <returns>The exit code of the command execution.</returns>
    Task<long> SendCommandAsync(ContainerListResponse container, string command);
}