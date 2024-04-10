using Avalonia.Interactivity;

namespace VirtualAutomation;

internal record RecordedEvent(
    RoutedEvent? RoutedEvent,
    object? Sender,
    RoutedEventArgs Args,
    RoutingStrategies Route,
    bool Handled,
    bool OriginalHandler);
