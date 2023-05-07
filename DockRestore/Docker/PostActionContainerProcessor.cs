namespace DockRestore.Docker;

/// <summary>
/// Represents a processor for performing post-action processing on Docker containers.
/// </summary>
public class PostActionContainerProcessor : BaseContainerProcessor, IPostActionContainerProcessor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PostActionContainerProcessor"/> class.
    /// </summary>
    /// <param name="dockerContainerManager">The Docker container manager.</param>
    /// <param name="filters">The container filters to apply.</param>
    public PostActionContainerProcessor(IDockerContainerManager dockerContainerManager, IEnumerable<IContainerFilter> filters) 
        : base(dockerContainerManager, filters)
    {
    }

    /// <summary>
    /// Gets the label key used to identify post-action commands in container labels.
    /// </summary>
    protected override string ActionLabel => "com.ingeniumignis.dockrestore.post_extract";
}