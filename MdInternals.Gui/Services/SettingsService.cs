using System.Text.Json;

namespace MdInternals.Gui.Services;

public class SettingsService
{
    public string Language { get; set; } = "ru";
    public string Theme { get; set; } = "light";
    public string DefaultEncoding { get; set; } = "utf-8";
    public string DefaultOutputPath { get; set; } = "";
    public string LogLevel { get; set; } = "INFO";

    private static string PathToSettings => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MdInternals", "settings.json");

    public static SettingsService Load(LoggerService logger)
    {
        try
        {
            if (!File.Exists(PathToSettings)) return new SettingsService();
            return JsonSerializer.Deserialize<SettingsService>(File.ReadAllText(PathToSettings)) ?? new SettingsService();
        }
        catch (Exception ex)
        {
            logger.Error("Failed to load settings", ex);
            return new SettingsService();
        }
    }

    public void Save(LoggerService logger)
    {
        try
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(PathToSettings)!);
            File.WriteAllText(PathToSettings, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
            logger.Info("Settings saved");
        }
        catch (Exception ex) { logger.Error("Failed to save settings", ex); }
    }
}
