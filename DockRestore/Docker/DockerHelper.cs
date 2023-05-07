using Docker.DotNet;
using Microsoft.Extensions.DependencyInjection;

namespace DockRestore.Docker;

/// <summary>
/// Provides helper methods to register the <see cref="IDockerContainerManager"/> service.
/// </summary>
public static class DockerHelper
{
    /// <summary>
    /// The Docker API endpoint for Unix sockets.
    /// </summary>
    private static Uri DockerUnixSocketEndpoint { get; } = new Uri("unix:///var/run/docker.sock");

    /// <summary>
    /// Adds the <see cref="IDockerContainerManager"/> implementation to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> after adding the service.</returns>
    public static IServiceCollection AddDockerContainerManager(this IServiceCollection services)
    {
        var isDockerAvailable = IsDockerAvailable();

        if (isDockerAvailable)
        {
            services.AddTransient<IDockerContainerManager>(_ => new DockerContainerManager(DockerUnixSocketEndpoint));
        }
        else
        {
            services.AddTransient<IDockerContainerManager, NullDockerContainerManager>();
        }

        return services;
    }

    /// <summary>
    /// Determines if Docker is available on the system.
    /// </summary>
    /// <returns>true if Docker is available, otherwise false.</returns>
    private static bool IsDockerAvailable()
    {
        try
        {
            var tempClient = new DockerClientConfiguration(DockerUnixSocketEndpoint).CreateClient();
            tempClient.System.PingAsync().GetAwaiter().GetResult();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
