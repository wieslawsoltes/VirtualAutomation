using System.Collections.Generic;
using Avalonia.Interactivity;

namespace VirtualAutomation;

internal record Recording(
    List<RecordedEvent> RecordedEvents,
    RoutedEventArgs Original)
{
    public RoutingStrategies Routes { get; set; }

    public bool IsHandled { get; set; }
}
