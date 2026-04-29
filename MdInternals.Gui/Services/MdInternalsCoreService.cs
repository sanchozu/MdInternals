using Elisy.MdInternals;

namespace MdInternals.Gui.Services;

public class MdInternalsCoreService
{
    private readonly LoggerService _logger;
    public MdInternalsCoreService(LoggerService logger) => _logger = logger;

    public async Task ExportToXmlAsync(string inputPath, string outputPath, bool extractCode, bool keepStructure, IProgress<int> progress, CancellationToken token)
    {
        await Task.Run(() =>
        {
            token.ThrowIfCancellationRequested();
            if (!File.Exists(inputPath)) throw new FileNotFoundException("Input file not found", inputPath);
            Directory.CreateDirectory(outputPath);

            using MetadataPackage package = CreatePackage(inputPath);
            package.Open(inputPath);

            token.ThrowIfCancellationRequested();
            progress.Report(40);
            _logger.Info($"Loaded package with {package.MetadataObjects.Count} objects");

            var outFile = Path.Combine(outputPath, "metadata-summary.txt");
            var lines = package.MetadataObjects.Select(o => $"{o.GetType().Name}:{o.ImageRow.FileName}").ToArray();

            token.ThrowIfCancellationRequested();
            File.WriteAllLines(outFile, lines);
            progress.Report(100);
            _logger.Warn("TODO: Full XML export/rebuild API is not exposed by MdInternals core.");
        }, token);
    }

    public Task<string> DecompileOpCodeAsync(string inputPath, string encoding)
    {
        _logger.Warn("TODO: Public OP-code decompiler API not found in core. Placeholder is returned.");
        return Task.FromResult("Decompiler is not available via public API yet.");
    }

    public Task<List<string>> TestAndListDbObjectsAsync()
    {
        _logger.Warn("TODO: MSSQL browser/export API is not exposed for GUI usage.");
        return Task.FromResult(new List<string> { "MSSQL connection test is currently unavailable in GUI wrapper." });
    }

    private static MetadataPackage CreatePackage(string path)
    {
        return Path.GetExtension(path).ToLowerInvariant() switch
        {
            ".cf" => new CfPackage(),
            ".cfu" => new CfuPackage(),
            ".epf" => new EpfPackage(),
            ".erf" => new ErfPackage(),
            _ => throw new NotSupportedException("Supported: .cf/.cfu/.epf/.erf")
        };
    }
}
