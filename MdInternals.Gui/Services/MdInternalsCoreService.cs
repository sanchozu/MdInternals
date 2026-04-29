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

            progress.Report(25);
            var fileInfo = new FileInfo(inputPath);
            var summaryLines = new List<string>
            {
                $"Input: {inputPath}",
                $"SizeBytes: {fileInfo.Length}",
                $"Extension: {fileInfo.Extension}",
                $"ExtractCode: {extractCode}",
                $"KeepStructure: {keepStructure}",
                "",
                "TODO: Direct core API integration is disabled in CI-safe build because legacy .NET Framework 4.0 reference assemblies are missing on GitHub runners.",
                "To enable full integration, retarget legacy core or build GUI in an environment with .NET Framework 4.0 targeting pack."
            };

            token.ThrowIfCancellationRequested();
            File.WriteAllLines(Path.Combine(outputPath, "metadata-summary.txt"), summaryLines);
            progress.Report(100);
            _logger.Warn("TODO: Full XML export/rebuild requires migration or dedicated legacy build environment.");
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
}
