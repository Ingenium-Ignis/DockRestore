namespace DockRestore.Docker;

/// <summary>
/// Represents a processor for performing pre-action processing on Docker containers.
/// </summary>
public class PreActionContainerProcessor : BaseContainerProcessor, IPreActionContainerProcessor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PreActionContainerProcessor"/> class.
    /// </summary>
    /// <param name="dockerContainerManager">The Docker container manager.</param>
    /// <param name="filters">The container filters to apply.</param>
    public PreActionContainerProcessor(IDockerContainerManager dockerContainerManager, IEnumerable<IContainerFilter> filters) 
        : base(dockerContainerManager, filters)
    {
    }

    /// <summary>
    /// Gets the label key used to identify pre-action commands in container labels.
    /// </summary>
    protected override string ActionLabel => "com.ingeniumignis.dockrestore.pre_extract";
}