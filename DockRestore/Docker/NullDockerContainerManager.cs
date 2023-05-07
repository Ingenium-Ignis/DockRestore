using Docker.DotNet.Models;

namespace DockRestore.Docker;

/// <summary>
/// A null implementation of the IDockerContainerManager interface.
/// This class is intended to be used when Docker is not available or not needed.
/// </summary>
public class NullDockerContainerManager : INullDockerContainerManager
{
    /// <summary>
    /// Asynchronously returns an empty list of Docker containers.
    /// </summary>
    /// <returns>An empty list of Docker containers.</returns>
    public async Task<IList<ContainerListResponse>> ListContainersAsync()
    {
        return await Task.FromResult(new List<ContainerListResponse>());
    }

    /// <summary>
    /// Returns an empty string as the current container ID.
    /// </summary>
    /// <returns>An empty string.</returns>
    public string GetCurrentContainerId()
    {
        return string.Empty;
    }

    /// <summary>
    /// Asynchronously returns a zero exit code, simulating a successful command execution.
    /// </summary>
    /// <param name="container">The target Docker container (ignored).</param>
    /// <param name="command">The command to send (ignored).</param>
    /// <returns>A zero exit code.</returns>
    public async Task<long> SendCommandAsync(ContainerListResponse container, string command)
    {
        return await Task.FromResult(0L);
    }
}
