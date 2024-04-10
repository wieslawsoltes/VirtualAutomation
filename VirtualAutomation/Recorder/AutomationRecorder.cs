using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace VirtualAutomation;

public class AutomationRecorder
{
    private Recording? _recording;

    public void AttachRecorder()
    {
        var routes = RoutingStrategies.Direct | RoutingStrategies.Tunnel | RoutingStrategies.Bubble;

        // InputElement.GotFocusEvent.AddClassHandler<InputElement>(OnGotFocus, routes, true);
        // InputElement.PointerPressedEvent.AddClassHandler<InputElement>(OnPointerPressed, routes, true);

        //((RoutedEvent)InputElement.PointerPressedEvent).AddClassHandler(typeof(object), OnRoutedEvent, routes, true);
        //InputElement.PointerPressedEvent.RouteFinished.Subscribe(HandleRouteFinished);

        var recordedEvents = new HashSet<RoutedEvent>()
        {
            Button.ClickEvent,
            InputElement.KeyDownEvent,
            InputElement.KeyUpEvent,
            InputElement.TextInputEvent,
            InputElement.PointerReleasedEvent,
            InputElement.PointerPressedEvent,

            Gestures.DoubleTappedEvent,
            InputElement.PointerWheelChangedEvent,

            //InputElement.GotFocusEvent,
            //InputElement.LostFocusEvent,
            //Gestures.ScrollGestureEvent,
            //Gestures.ScrollGestureEndedEvent,
        };

        // RaiseEvent(new PointerPressedEventArgs());

        var nodes = RoutedEventRegistry.Instance.GetAllRegistered()
            .GroupBy(e => e.OwnerType)
            .OrderBy(e => e.Key.Name)
            .Select(g => new { Type = g.Key, Events = g.OrderBy(e => e.Name) })
            .ToArray();

        foreach (var node in nodes)
        {
            foreach (var routedEvent in node.Events)
            {
                if (recordedEvents.Contains(routedEvent))
                {
                    routedEvent.AddClassHandler(typeof(object), OnRoutedEvent, routes, true);
                    routedEvent.RouteFinished.Subscribe(HandleRouteFinished);
                }
            }
        }
    }

    private void HandleRouteFinished(RoutedEventArgs e)
    {
        if (_recording is not null)
        {
            // Check if current recording has all RoutedEvent registered RoutingStrategies

            //Console.ForegroundColor = ConsoleColor.DarkGray;
            //Console.WriteLine($"[Finished] {e.RoutedEvent.Name}, {e.Route}");

            if (_recording.Routes == e.RoutedEvent.RoutingStrategies)
            {
                //Console.ForegroundColor = ConsoleColor.Blue;
                //Console.WriteLine($"[End] {e.RoutedEvent.Name}, Routes={_recording.Routes}");
                //Console.ForegroundColor = ConsoleColor.Black;

                var originalHandlerRoutedEvent = _recording.RecordedEvents.FirstOrDefault(x => x.OriginalHandler);
                if (originalHandlerRoutedEvent is not null)
                {
                    Print(originalHandlerRoutedEvent);
                }
                else
                {
                    var lastRoutedEvent = _recording.RecordedEvents.LastOrDefault();
                    if (lastRoutedEvent is not null)
                    {
                        Print(lastRoutedEvent);
                    }
                }

                //foreach (var recordedEvent in _recording.RecordedEvents)
                //{
                //    Print(recordedEvent);
                //}

                //Console.WriteLine("");
                //Console.WriteLine("");

                _recording = null;
            }
        }
    }

    private void OnRoutedEvent(object? sender, RoutedEventArgs e)
    {
        if (_recording is null || _recording.Original != e)
        {
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine($"[Start] {e.RoutedEvent.Name}, RoutingStrategies={e.RoutedEvent?.RoutingStrategies}");

            _recording = new(new List<RecordedEvent>(), e);
        }

        var originalHandler = false;

        if (!_recording.IsHandled && e.Handled)
        {
            _recording.IsHandled = true;
            originalHandler = true;
        }

        _recording.RecordedEvents.Add(new RecordedEvent(e.RoutedEvent, sender, e, e.Route, e.Handled, originalHandler));

        if (!_recording.Routes.HasFlag(e.Route))
        {
            _recording.Routes |= e.Route;
        }
    }

    private static void Print(RecordedEvent recordedEvent)
    {
        Console.ForegroundColor = recordedEvent.Handled
            ? (recordedEvent.OriginalHandler
                ? ConsoleColor.Magenta
                : ConsoleColor.Green)
            : ConsoleColor.Gray;

        var eventName = $"{recordedEvent.RoutedEvent.OwnerType.Name}.{recordedEvent.RoutedEvent.Name}";

        if (recordedEvent.RoutedEvent == InputElement.PointerPressedEvent)
        {
            if (recordedEvent.Sender is StyledElement styledElement and InputElement inputElement)
            {
                var name = AutomationProperties.GetName(styledElement);
                var position = (recordedEvent.Args as PointerPressedEventArgs).GetPosition(inputElement);
                if (!string.IsNullOrEmpty(name))
                {
                    Console.WriteLine(
                        $"[{eventName}] {styledElement}, name={name}, Handled={recordedEvent.Handled}, position={position}, {recordedEvent.Route}");
                }
                else
                {
                    Console.WriteLine(
                        $"[{eventName}] {styledElement},.Handled={recordedEvent.Handled}, position={position}, {recordedEvent.Route}");
                }
            }
        }
        else
        {
            Console.WriteLine($"[{eventName}] {recordedEvent.Sender}, Handled={recordedEvent.Handled}, {recordedEvent.Route}");
        }
    }

    private void OnGotFocus(object? sender, GotFocusEventArgs e)
    {
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
    }
}
