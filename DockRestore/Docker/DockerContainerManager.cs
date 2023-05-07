using System.Net;
using System.Text.RegularExpressions;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace DockRestore.Docker;

/// <summary>
/// Provides functionality to manage Docker containers.
/// </summary>
public class DockerContainerManager : IDockerContainerManager
{
    private readonly DockerClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerContainerManager"/> class with the specified endpoint.
    /// </summary>
    /// <param name="endpoint">The Docker API endpoint.</param>
    public DockerContainerManager(Uri endpoint)
    {
        _client = new DockerClientConfiguration(endpoint).CreateClient();
    }

    /// <inheritdoc/>
    public async Task<IList<ContainerListResponse>> ListContainersAsync()
    {
        return await _client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
    }

    /// <inheritdoc/>
    public string GetCurrentContainerId()
    {
        return Dns.GetHostName();
    }

    /// <inheritdoc/>
    public async Task<long> SendCommandAsync(ContainerListResponse container, string command)
    {
        var inspection = await _client.Containers.InspectContainerAsync(container.ID);
        var tty = inspection.Config.Tty;

        // Parse the command string into separate arguments
        var cmd = Regex
            .Matches(command, @"(?<=^|\s)(?:""[^""\\]*(?:\\.[^""\\]*)*""|'[^'\\]*(?:\\.[^'\\]*)*'|[^""'\s]+)")
            .Select(x => x.Value)
            .Where(x => !string.IsNullOrEmpty(x))
            .ToArray();

        // Remove quotes from the arguments if needed
        cmd = cmd.Select(x =>
        {
            if (x.StartsWith('"') && x.EndsWith('"'))
            {
                return x.Trim('"');
            }

            if (x.StartsWith('\'') && x.EndsWith('\''))
            {
                return x.Trim('\'');
            }

            return x;
        }).ToArray();
        
        var execConfig = new ContainerExecCreateParameters
        {
            AttachStderr = true,
            AttachStdout = true,
            Cmd = cmd,
            Tty = tty
        };
        
        var execCreateResponse = await _client.Exec.ExecCreateContainerAsync(container.ID, execConfig);

        using var stdOutAndErrStream = await _client.Exec.StartAndAttachContainerExecAsync(execCreateResponse.ID, false);
        var (stdout, stderr) = await stdOutAndErrStream.ReadOutputToEndAsync(default);

        var execInspectResponse = await _client.Exec.InspectContainerExecAsync(execCreateResponse.ID);

        var exitCode = execInspectResponse.ExitCode;

        return exitCode;
    }
}