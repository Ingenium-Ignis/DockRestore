using DockRestore.ConsoleCommands;
using DockRestore.Docker;
using DockRestore.FileRestoreManager;
using DockRestore.Infrastructure.Notification;
using DockRestore.Infrastructure.SpectreConsoleCli;
using DockRestore.InputManager;
using DockRestore.LiveDisplayManager;
using DockRestore.Models;
using DockRestore.Validator;
using DockRestore.ViewModels;
using DockRestore.ViewModels.SelectedItemNotifications;
using DockRestore.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var services = AddServices(configuration);

// Create a type registrar and register any dependencies.
// A type registrar is an adapter for a DI framework.
var registrar = new TypeRegistrar(services);

// Create a new command app with the registrar
// and run it with the provided arguments.
var app = new CommandApp<DefaultCommand>(registrar);

app.Configure(appConfiguration => appConfiguration
    .SetApplicationName("dockrestore")
    .SetApplicationVersion("1.0")
);

return await app.RunAsync(args);

IServiceCollection AddServices(IConfiguration configurationRoot)
{
    var archiveFolder = configurationRoot.GetSection("Backup")["Archive"] ?? "/archive";
    var restoreTarget = configurationRoot.GetSection("Backup")["RestoreTarget"] ?? "/restore-target";

    var serviceCollection = new ServiceCollection()
        .AddTransient<IInputManager, InputManager>()
        .AddTransient<IDirectoryContentInfo>(_ => new DirectoryContentInfo(archiveFolder))
        .AddTransient<IDirectoryContentsViewModel, DirectoryContentsViewModel>()
        .AddTransient<IValidator, Validator>()
        .AddTransient<IFileRestoreManager>(provider =>
            new FileRestoreManager(provider.GetRequiredService<IValidator>(), restoreTarget))
        .AddTransient<IContainerFilter, RunningContainersFilter>()
        .AddTransient<IContainerFilter, SameComposeProjectContainersFilter>()
        .AddTransient<IPreActionContainerProcessor, PreActionContainerProcessor>()
        .AddTransient<IPostActionContainerProcessor, PostActionContainerProcessor>()
        .AddTransient<IRestoreProcessViewModel, RestoreProcessViewModel>()
        .AddTransient<IRestoreProcessView, RestoreProcessView>()
        .AddTransient<ILiveDisplayManager, LiveDisplayManager>()
        .AddTransient<DirectoryContentsView>()
        .AddTransient<IDirectoryContentsView>(provider => provider.GetRequiredService<DirectoryContentsView>())
        .AddTransient<INotificationHandler<InputNotification>>(provider =>
            provider.GetRequiredService<DirectoryContentsView>())
        .AddTransient<ArchiveSelectedNotificationHandler>()
        .AddTransient<IArchiveSelectedNotificationHandler>(provider =>
            provider.GetRequiredService<ArchiveSelectedNotificationHandler>())
        .AddTransient<INotificationHandler<ArchiveSelectedNotification>>(provider =>
            provider.GetRequiredService<ArchiveSelectedNotificationHandler>())
        .AddSingleton(configurationRoot)
        .AddDockerContainerManager();
    
    return serviceCollection;
}
