using Docker.DotNet.Models;

namespace DockRestore.Docker;

/// <summary>
/// Represents a filter for filtering a collection of Docker containers.
/// </summary>
public interface IContainerFilter
{
    /// <summary>
    /// Filters the specified collection of <see cref="ContainerListResponse"/> objects.
    /// </summary>
    /// <param name="containers">The collection of <see cref="ContainerListResponse"/> objects to filter.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of filtered <see cref="ContainerListResponse"/> objects.</returns>
    IEnumerable<ContainerListResponse> Filter(IEnumerable<ContainerListResponse> containers);
}