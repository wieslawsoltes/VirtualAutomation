using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Automation.Peers;
using Avalonia.Controls;

namespace VirtualAutomation;

public class Entity
{
    public Entity(string type, Rect bounds)
    {
        Type = type;
        Bounds = bounds;
    }

    public string Type { get; set; }

    public Rect Bounds { get; set; }
}

public class EntityAutomationPeer : AutomationPeer
{
    private readonly Entity _entity;
    private AutomationPeer? _parent;

    public EntityAutomationPeer(Entity entity, AutomationPeer parent)
    {
        _entity = entity;
        _parent = parent;
    }

    protected override IReadOnlyList<AutomationPeer> GetOrCreateChildrenCore()
    {
        return Array.Empty<AutomationPeer>();
    }

    protected override string GetClassNameCore()
    {
        return "Entity";
    }

    protected override AutomationPeer? GetLabeledByCore()
    {
        // ignored
        return null;
    }

    protected override string? GetNameCore()
    {
        return _entity.Type;
    }

    protected override AutomationPeer? GetParentCore()
    {
        return _parent;
    }

    protected override bool HasKeyboardFocusCore()
    {
        // ignored
        return false;
    }

    protected override bool IsContentElementCore()
    {
        // ignored
        return false;
    }

    protected override bool IsControlElementCore()
    {
        // ignored
        return false;
    }

    protected override bool IsEnabledCore()
    {
        // ignored
        return true;
    }

    protected override bool IsKeyboardFocusableCore()
    {
        // ignored
        return false;
    }

    protected override void SetFocusCore()
    {
        // ignored
    }

    protected override bool ShowContextMenuCore()
    {
        // ignored
        return false;
    }

    protected override bool TrySetParent(AutomationPeer? parent)
    {
        _parent = parent;
        return true;
    }

    protected override void BringIntoViewCore()
    {
        // ignored
    }

    protected override string? GetAcceleratorKeyCore()
    {
        // ignored
        return null;
    }

    protected override string? GetAccessKeyCore()
    {
        // ignored
        return null;
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.ListItem;
    }

    protected override string? GetAutomationIdCore()
    {
        // TODO:
        return null;
    }

    protected override Rect GetBoundingRectangleCore()
    {
        // TODO:
        return default;
    }

    // Implement other necessary methods and properties, including any pattern support
}

public class DiagramAutomationPeer : ControlAutomationPeer
{
    private List<Entity> _entities;
    
    public DiagramAutomationPeer(Control owner) : base(owner)
    {
        _entities = new List<Entity>
        {
           new Entity("Line", new Rect(0, 0, 100, 100)),
           new Entity("Rectangle", new Rect(200, 0, 70, 70)),
           new Entity("Path", new Rect(0, 200, 50, 50)),
        };
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.List;
    }

    // Override methods as necessary, similar to the virtual item peer
    protected override List<AutomationPeer> GetChildrenCore()
    {
        // TODO: Cache children
        var children = new List<AutomationPeer>();

        foreach (var entity in _entities)
        {
            children.Add(new EntityAutomationPeer(entity, this));
        }

        return children;
    }
}

public class DiagramViewer : Border
{
    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new DiagramAutomationPeer(this);
    }
}

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
}
