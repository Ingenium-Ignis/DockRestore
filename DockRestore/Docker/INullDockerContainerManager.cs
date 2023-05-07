namespace DockRestore.Docker;

/// <summary>
/// A null implementation of the IDockerContainerManager interface.
/// This interface is intended to be used when Docker is not available or not needed.
/// </summary>
public interface INullDockerContainerManager : IDockerContainerManager
{
}
