using System.Text;

namespace MdInternals.Gui.Services;

public class LoggerService
{
    private readonly Queue<string> _recentLines = new();
    private const int MaxRecentLines = 1000;
    private readonly string _logDir;
    private readonly string _dailyLogFilePath;
    public event EventHandler<string>? OnLog;

    public LoggerService()
    {
        _logDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MdInternals", "logs");
        Directory.CreateDirectory(_logDir);
        _dailyLogFilePath = Path.Combine(_logDir, $"mdinternals-{DateTime.Now:yyyyMMdd}.log");
    }

    public void Info(string msg) => Write("INFO", msg);
    public void Warn(string msg) => Write("WARN", msg);
    public void Error(string msg, Exception ex) => Write("ERROR", $"{msg}: {ex.Message}");

    private void Write(string level, string msg)
    {
        var line = $"[{DateTime.Now:O}] {level} {msg}";
        _recentLines.Enqueue(line);
        if (_recentLines.Count > MaxRecentLines)
            _recentLines.Dequeue();

        File.AppendAllText(_dailyLogFilePath, line + Environment.NewLine, Encoding.UTF8);
        OnLog?.Invoke(this, line);
    }

    public void Clear()
    {
        _recentLines.Clear();
        File.WriteAllText(_dailyLogFilePath, string.Empty, Encoding.UTF8);
    }

    public void Export()
    {
        var exportPath = Path.Combine(_logDir, $"exported-log-{DateTime.Now:yyyyMMdd-HHmmss}.txt");
        File.Copy(_dailyLogFilePath, exportPath, overwrite: true);
    }
}
