using Docker.DotNet.Models;

namespace DockRestore.Docker;

/// <summary>
/// Represents a filter that selects Docker containers belonging to the same Docker Compose project as the current container.
/// </summary>
public class SameComposeProjectContainersFilter : IContainerFilter
{
    private readonly (string? Project, string? WorkingDirectory, string? ConfigFiles) _hostProject;

    /// <summary>
    /// Initializes a new instance of the <see cref="SameComposeProjectContainersFilter"/> class.
    /// </summary>
    /// <param name="clientWrapper">The Docker container manager.</param>
    public SameComposeProjectContainersFilter(IDockerContainerManager clientWrapper)
    {
        var currentContainerId = clientWrapper.GetCurrentContainerId();
        var hostContainer = clientWrapper.ListContainersAsync().Result.SingleOrDefault(c => c.ID.StartsWith(currentContainerId));
        
        if (hostContainer != null)
        {
            _hostProject = GetDockerComposeProject(hostContainer.Labels);
        }
    }

    /// <inheritdoc/>
    public IEnumerable<ContainerListResponse> Filter(IEnumerable<ContainerListResponse> containers)
    {
        return containers.Where(c => GetDockerComposeProject(c.Labels) == _hostProject);
    }
    
    /// <summary>
    /// Gets the Docker Compose project information from the specified container labels.
    /// </summary>
    /// <param name="labels">The dictionary containing the container labels.</param>
    /// <returns>A tuple containing the project name, working directory, and name of configuration files.</returns>
    private (string? Project, string? WorkingDirectory, string? ConfigFiles) GetDockerComposeProject(IDictionary<string, string> labels)
    {
        string? project = null;
        string? workingDirectory = null;
        string? configFiles = null;

        if (labels.TryGetValue("com.docker.compose.project", out var label))
        {
            project = label;
        }

        if (labels.TryGetValue("com.docker.compose.project.working_dir", out label))
        {
            workingDirectory = label;
        }

        if (labels.TryGetValue("com.docker.compose.project.config_files", out label))
        {
            configFiles = label;
        }

        return (project, workingDirectory, configFiles);
    }
}