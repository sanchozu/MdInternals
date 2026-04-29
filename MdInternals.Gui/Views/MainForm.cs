using MdInternals.Gui.ViewModels;

namespace MdInternals.Gui.Views;

public class MainForm : Form
{
    private readonly MainViewModel _vm;
    private readonly TextBox _logBox = new() { Multiline = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Fill };
    private readonly Label _status = new() { Dock = DockStyle.Bottom, Height = 24, Text = "Ready" };
    private readonly ProgressBar _progress = new() { Dock = DockStyle.Bottom, Height = 18 };

    public MainForm(MainViewModel vm)
    {
        _vm = vm;
        Text = "MdInternals GUI";
        Width = 1200;
        Height = 800;

        var tabs = new TabControl { Dock = DockStyle.Fill };
        tabs.TabPages.Add(BuildConversionTab());
        tabs.TabPages.Add(BuildDecompilerTab());
        tabs.TabPages.Add(BuildDbTab());
        tabs.TabPages.Add(BuildSettingsTab());
        tabs.TabPages.Add(BuildAboutTab());

        Controls.Add(tabs);
        Controls.Add(_logBox);
        Controls.Add(_progress);
        Controls.Add(_status);

        _vm.LogUpdated += (_, m) => BeginInvoke(() => _logBox.AppendText(m + Environment.NewLine));
        _vm.StatusUpdated += (_, m) => BeginInvoke(() => _status.Text = m);
        _vm.ProgressUpdated += (_, p) => BeginInvoke(() => _progress.Value = Math.Clamp(p, 0, 100));
    }

    private TabPage BuildConversionTab()
    {
        var page = new TabPage("Конвертация форматов");
        var run = new Button { Text = "Экспорт в XML", Left = 10, Top = 10, Width = 150 };
        var cancel = new Button { Text = "Отмена", Left = 170, Top = 10, Width = 100 };
        var input = new TextBox { Left = 10, Top = 45, Width = 600, PlaceholderText = "Путь к .cf/.cfu/.epf/.erf" };
        var output = new TextBox { Left = 10, Top = 75, Width = 600, PlaceholderText = "Папка вывода" };
        run.Click += async (_, _) => await _vm.RunConvertAsync(input.Text, output.Text);
        cancel.Click += (_, _) => _vm.CancelCurrent();
        page.Controls.AddRange([run, cancel, input, output]);
        return page;
    }

    private TabPage BuildDecompilerTab()
    {
        var page = new TabPage("Декомпилятор ОП-кода");
        var input = new TextBox { Left = 10, Top = 10, Width = 600, PlaceholderText = "Файл" };
        var run = new Button { Left = 620, Top = 10, Width = 120, Text = "Декомпилировать" };
        var result = new TextBox { Left = 10, Top = 45, Width = 900, Height = 500, Multiline = true, ScrollBars = ScrollBars.Both };
        run.Click += async (_, _) => result.Text = await _vm.RunDecompilerAsync(input.Text);
        page.Controls.AddRange([input, run, result]);
        return page;
    }

    private TabPage BuildDbTab()
    {
        var page = new TabPage("Подключение к БД 1С");
        var test = new Button { Left = 10, Top = 10, Width = 150, Text = "Тест соединения" };
        var password = new TextBox { Left = 10, Top = 45, Width = 250, UseSystemPasswordChar = true, PlaceholderText = "Пароль" };
        var tree = new TreeView { Left = 10, Top = 80, Width = 450, Height = 450 };
        test.Click += async (_, _) =>
        {
            tree.Nodes.Clear();
            var items = await _vm.GetDbObjectsAsync();
            foreach (var item in items) tree.Nodes.Add(item);
        };
        page.Controls.AddRange([test, password, tree]);
        return page;
    }

    private TabPage BuildSettingsTab()
    {
        var page = new TabPage("Настройки");
        var save = new Button { Text = "Сохранить настройки", Left = 10, Top = 10 };
        save.Click += (_, _) => _vm.SaveSettings();
        page.Controls.Add(save);
        return page;
    }

    private TabPage BuildAboutTab()
    {
        var page = new TabPage("О программе и логи");
        var info = new Label
        {
            Left = 10,
            Top = 10,
            Width = 900,
            Height = 100,
            Text = "MdInternals GUI\nLicense: GPL-3.0\nRepository: https://github.com/sanchozu/MdInternals"
        };
        var clear = new Button { Left = 10, Top = 120, Text = "Очистить лог" };
        var export = new Button { Left = 120, Top = 120, Text = "Экспорт лога" };
        clear.Click += (_, _) => _vm.ClearLog();
        export.Click += (_, _) => _vm.ExportLog();
        page.Controls.AddRange([info, clear, export]);
        return page;
    }
}
