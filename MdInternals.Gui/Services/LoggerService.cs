using System.Text;

namespace MdInternals.Gui.Services;

public class LoggerService
{
    private readonly List<string> _lines = new();
    private readonly string _logDir;
    public event EventHandler<string>? OnLog;

    public LoggerService()
    {
        _logDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MdInternals", "logs");
        Directory.CreateDirectory(_logDir);
    }

    public void Info(string msg) => Write("INFO", msg);
    public void Warn(string msg) => Write("WARN", msg);
    public void Error(string msg, Exception ex) => Write("ERROR", $"{msg}: {ex.Message}");

    private void Write(string level, string msg)
    {
        var line = $"[{DateTime.Now:O}] {level} {msg}";
        _lines.Add(line);
        File.AppendAllText(Path.Combine(_logDir, $"mdinternals-{DateTime.Now:yyyyMMdd}.log"), line + Environment.NewLine, Encoding.UTF8);
        OnLog?.Invoke(this, line);
    }

    public void Clear() => _lines.Clear();
    public void Export() => File.WriteAllLines(Path.Combine(_logDir, "exported-log.txt"), _lines);
}
