using Docker.DotNet.Models;

namespace DockRestore.Docker
{
    /// <summary>
    /// A base class for processing Docker containers based on their labels.
    /// </summary>
    public abstract class BaseContainerProcessor
    {
        private readonly IDockerContainerManager _dockerContainerManager;
        private readonly IEnumerable<IContainerFilter> _filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseContainerProcessor"/> class.
        /// </summary>
        /// <param name="dockerContainerManager">The Docker container manager used for interacting with Docker containers.</param>
        /// <param name="filters">A collection of filters to apply to the containers before processing.</param>
        protected BaseContainerProcessor(IDockerContainerManager dockerContainerManager, IEnumerable<IContainerFilter> filters)
        {
            _dockerContainerManager = dockerContainerManager;
            _filters = filters;
        }
        
        /// <summary>
        /// Gets the label name used to identify the action to perform on the container.
        /// </summary>
        protected abstract string ActionLabel { get; }

        /// <summary>
        /// Processes the specified Docker containers after applying the filters.
        /// </summary>
        /// <param name="containers">The Docker containers to process.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains a boolean value indicating whether the processing was successful.</returns>
        public async Task<bool> ProcessAsync(IEnumerable<ContainerListResponse> containers)
        {
            var success = true;
            // Apply filters
            containers = _filters.Aggregate(containers, (current, filter) => filter.Filter(current));

            // Process filtered containers
            foreach (var container in containers)
            {
                if (!container.Labels.TryGetValue(ActionLabel, out var command)) continue;
                if (await _dockerContainerManager.SendCommandAsync(container, command) != 0)
                {
                    success = false;
                }
            }

            return success;
        }
    }
}