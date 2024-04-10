using Avalonia.Automation.Peers;
using Avalonia.Controls;

namespace VirtualAutomation;

public class DiagramViewer : Border
{
    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new DiagramAutomationPeer(this);
    }
}
