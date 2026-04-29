using MdInternals.Gui.Services;

namespace MdInternals.Gui.ViewModels;

public class MainViewModel
{
    private readonly LoggerService _logger;
    private readonly SettingsService _settings;
    private readonly MdInternalsCoreService _core;
    private CancellationTokenSource? _cts;

    public event EventHandler<string>? LogUpdated;
    public event EventHandler<string>? StatusUpdated;
    public event EventHandler<int>? ProgressUpdated;

    public MainViewModel(LoggerService logger, SettingsService settings, MdInternalsCoreService core)
    {
        _logger = logger;
        _settings = settings;
        _core = core;
        _logger.OnLog += (_, m) => LogUpdated?.Invoke(this, m);
    }

    public async Task RunConvertAsync(string input, string output)
    {
        _cts = new CancellationTokenSource();
        StatusUpdated?.Invoke(this, "Conversion in progress");
        var progress = new Progress<int>(p => ProgressUpdated?.Invoke(this, p));
        try { await _core.ExportToXmlAsync(input, output, true, true, progress, _cts.Token); }
        catch (OperationCanceledException) { _logger.Warn("Operation cancelled."); }
        catch (Exception ex) { _logger.Error("Conversion failed", ex); }
        StatusUpdated?.Invoke(this, "Ready");
    }

    public Task<string> RunDecompilerAsync(string input) => _core.DecompileOpCodeAsync(input, _settings.DefaultEncoding);
    public async Task TestDbAsync(TreeView tree)
    {
        tree.Nodes.Clear();
        var items = await _core.TestAndListDbObjectsAsync();
        foreach (var item in items) tree.Nodes.Add(item);
    }
    public void CancelCurrent() => _cts?.Cancel();
    public void SaveSettings() => _settings.Save(_logger);
    public void ClearLog() => _logger.Clear();
    public void ExportLog() => _logger.Export();
}
