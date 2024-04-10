using Avalonia;
using Avalonia.Controls;

namespace VirtualAutomation;

public partial class MainWindow : Window
{
    private readonly AutomationRecorder _automationRecorder = new();

    public MainWindow()
    {
        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif

        _automationRecorder.AttachRecorder();
    }
}
