using Docker.DotNet.Models;

namespace DockRestore.Docker;

/// <summary>
/// Represents a filter that selects only running Docker containers.
/// </summary>
public class RunningContainersFilter : IContainerFilter
{
    /// <inheritdoc/>
    public IEnumerable<ContainerListResponse> Filter(IEnumerable<ContainerListResponse> containers)
    {
        return containers.Where(c => c.State == "running");
    }
}