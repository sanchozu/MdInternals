using MdInternals.Gui.Services;
using MdInternals.Gui.ViewModels;
using MdInternals.Gui.Views;

namespace MdInternals.Gui;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        var logger = new LoggerService();
        var settings = SettingsService.Load(logger);
        var core = new MdInternalsCoreService(logger);
        var vm = new MainViewModel(logger, settings, core);
        Application.Run(new MainForm(vm));
    }
}
