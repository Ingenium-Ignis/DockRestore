using DockRestore.ViewModels;
using Spectre.Console;

namespace DockRestore.Views;

/// <summary>
/// Represents the view for the restore process, displaying the restoration progress and handling errors.
/// </summary>
public class RestoreProcessView : IRestoreProcessView
{
    private readonly IRestoreProcessViewModel _viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="RestoreProcessView"/> class.
    /// </summary>
    /// <param name="viewModel">The view model instance.</param>
    public RestoreProcessView(IRestoreProcessViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    /// <inheritdoc/>
    public async Task ShowAsync(FileInfo selectedArchive)
    {
        AnsiConsole.Write($"Restoring from {selectedArchive.Name} ... ");
        try
        {
            bool result;

            if (!_viewModel.IsDockerContainerManagerNull())
            {
                result = await _viewModel.RunPreActionProcessorAsync();
                if (!result) throw new Exception("Pre-extraction command failed.");
            }

            _viewModel.RestoreFromArchive(selectedArchive);

            if (!_viewModel.IsDockerContainerManagerNull())
            {
                result = await _viewModel.RunPostActionProcessorAsync();
                if (!result) throw new Exception("Post-extraction command failed.");
            }

            AnsiConsole.Write(new Markup("[green]done[/]\n"));
        }
        catch (Exception ex)
        {
            AnsiConsole.Write(new Markup("[red]failed[/]\n"));
            AnsiConsole.Write(new Markup($"[red][[Error]][/] Backup couldn't be restored: {Markup.Escape(ex.InnerException?.Message ?? ex.Message)}\n"));
        }
    }
}